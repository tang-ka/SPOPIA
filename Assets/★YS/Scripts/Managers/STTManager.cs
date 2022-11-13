using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEngine.Networking;
using System.Collections;

public class STTManager : MonoBehaviour
{
    string micID;
    AudioClip clip;
    int recordingLength = 10; // 처음 Microphone.Start 로 보낼 고정변수(녹음시간)
    int recordingHZ = 22050;
	const int blockSize_16bit = 2;

	// Start is called before the first frame update
	void Start()
    {
        micID = Microphone.devices[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRecording()
    {
        Debug.Log("녹음 시작");
        clip = Microphone.Start(micID, false, recordingLength, recordingHZ);
    }

    public void StopRecording()
    {
        if(Microphone.IsRecording(micID))
        {
            Microphone.End(micID);
            Debug.Log("녹음 끝");

            // 실제 녹음 시간에 맞춰서 clip 데이터 재설정
            AudioClip temp = clip;
            float realLength = clip.length;
            float realSamples = clip.samples;
            float perSec = realSamples / realLength;
            float[] samples = new float[(int)(perSec * recordingLength)];
            temp.GetData(samples, 0);
            clip.SetData(samples, 0);

            if(clip == null)
            {
                Debug.Log("녹음 안됨");
                return;
            }

			// clip을 byteArray로 변환
			byte[] byteArray = ClipToByteArray(clip);

			// clip을 stt 서버로 보내기
			StartCoroutine(ToSever(url, byteArray));
		}
    }

    byte[] ClipToByteArray(AudioClip audioClip)
    {
        // 데이터를 byteArray로 보내기 위한 메모리스트림 사용
        MemoryStream memoryStream = new MemoryStream();
        int headerSize = 44;
        ushort bitSize = 16;

        int fileSize = audioClip.samples * blockSize_16bit + headerSize;

		// clip 데이터들을 filestream에 추가
		WriteFileHeader(ref memoryStream, fileSize);
		WriteFileFormat(ref memoryStream, audioClip.channels, audioClip.frequency, bitSize);
		WriteFileData(ref memoryStream, audioClip, bitSize);

		// filestream을 array 형태로 변환
		byte[] bytes = memoryStream.ToArray();

		return bytes;
    }

	private static int WriteFileHeader(ref MemoryStream stream, int fileSize)
	{
		int count = 0;
		int total = 12;

		// riff chunk id
		byte[] riff = Encoding.ASCII.GetBytes("RIFF");
		count += WriteBytesToMemoryStream(ref stream, riff, "ID");

		// riff chunk size
		int chunkSize = fileSize - 8; // total size - 8 for the other two fields in the header
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(chunkSize), "CHUNK_SIZE");

		byte[] wave = Encoding.ASCII.GetBytes("WAVE");
		count += WriteBytesToMemoryStream(ref stream, wave, "FORMAT");

		// Validate header
		Debug.AssertFormat(count == total, "Unexpected wav descriptor byte count: {0} == {1}", count, total);

		return count;
	}

    private static int WriteFileFormat(ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
	{
		int count = 0;
		int total = 24;

		byte[] id = Encoding.ASCII.GetBytes("fmt ");
		count += WriteBytesToMemoryStream(ref stream, id, "FMT_ID");

		int subchunk1Size = 16; // 24 - 8
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk1Size), "SUBCHUNK_SIZE");

		UInt16 audioFormat = 1;
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(audioFormat), "AUDIO_FORMAT");

		UInt16 numChannels = Convert.ToUInt16(channels);
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(numChannels), "CHANNELS");

		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(sampleRate), "SAMPLE_RATE");

		int byteRate = sampleRate * channels * BytesPerSample(bitDepth);
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(byteRate), "BYTE_RATE");

		UInt16 blockAlign = Convert.ToUInt16(channels * BytesPerSample(bitDepth));
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(blockAlign), "BLOCK_ALIGN");

		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(bitDepth), "BITS_PER_SAMPLE");

		// Validate format
		Debug.AssertFormat(count == total, "Unexpected wav fmt byte count: {0} == {1}", count, total);

		return count;
	}

	private static int WriteFileData(ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
	{
		int count = 0;
		int total = 8;

		// Copy float[] data from AudioClip
		float[] data = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData(data, 0);

		byte[] bytes = ConvertAudioClipDataToInt16ByteArray(data);

		byte[] id = Encoding.ASCII.GetBytes("data");
		count += WriteBytesToMemoryStream(ref stream, id, "DATA_ID");

		int subchunk2Size = Convert.ToInt32(audioClip.samples * blockSize_16bit); // BlockSize (bitDepth)
		count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk2Size), "SAMPLES");

		// Validate header
		Debug.AssertFormat(count == total, "Unexpected wav data id byte count: {0} == {1}", count, total);

		// Write bytes to stream
		count += WriteBytesToMemoryStream(ref stream, bytes, "DATA");

		// Validate audio data
		Debug.AssertFormat(bytes.Length == subchunk2Size, "Unexpected AudioClip to wav subchunk2 size: {0} == {1}", bytes.Length, subchunk2Size);

		return count;
	}

	private static byte[] ConvertAudioClipDataToInt16ByteArray(float[] data)
	{
		MemoryStream dataStream = new MemoryStream();

		int x = sizeof(Int16);

		Int16 maxValue = Int16.MaxValue;

		int i = 0;
		while (i < data.Length)
		{
			dataStream.Write(BitConverter.GetBytes(Convert.ToInt16(data[i] * maxValue)), 0, x);
			++i;
		}
		byte[] bytes = dataStream.ToArray();

		// Validate converted bytes
		Debug.AssertFormat(data.Length * x == bytes.Length, "Unexpected float[] to Int16 to byte[] size: {0} == {1}", data.Length * x, bytes.Length);

		dataStream.Dispose();

		return bytes;
	}

	private static int WriteBytesToMemoryStream(ref MemoryStream stream, byte[] bytes, string tag = "")
	{
		int count = bytes.Length;
		stream.Write(bytes, 0, count);

		return count;
	}

	private static int BytesPerSample(UInt16 bitDepth)
	{
		return bitDepth / 8;
	}

	// JSON
	[Serializable]
	public class VoiceRecognize
	{
		public string text;
	}

	// 사용할 언어 선택
	static string lang = "Kor";    // 언어 코드 ( Kor, Jpn, Eng, Chn )
	string url = $"https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang={lang}";

	private IEnumerator ToSever(string url, byte[] data)
	{
		// request 생성
		WWWForm form = new WWWForm();
		UnityWebRequest request = UnityWebRequest.Post(url, form);

		// 요청 헤더 설정 (네이버 클라우드 ID, Secret Key)
		request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", "nu4mc31ajs");
		request.SetRequestHeader("X-NCP-APIGW-API-KEY", "GkJTxkR2yrwOz9UDxaXoqqXR9ooxUGCqCrAxHRIH");
		request.SetRequestHeader("Content-Type", "application/octet-stream");

		// data 담기
		request.uploadHandler = new UploadHandlerRaw(data);

		// 응답 요청
		yield return request.SendWebRequest();

		// response가 없으면 eroor메세지 띄움
		if (request == null)
		{
			Debug.LogError(request.error);
		}
		else
		{
			// json 형태로 받음 {"text":"인식결과"}
			string message = request.downloadHandler.text;
			VoiceRecognize voiceRecognize = JsonUtility.FromJson<VoiceRecognize>(message);

			Debug.Log("Voice Server responded: " + voiceRecognize.text);
			// Voice Server responded: 인식결과
		}
	}
}
