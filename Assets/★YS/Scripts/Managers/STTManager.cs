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
    int recordingLength = 10; // ó�� Microphone.Start �� ���� ��������(�����ð�)
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
        Debug.Log("���� ����");
        clip = Microphone.Start(micID, false, recordingLength, recordingHZ);
    }

    public void StopRecording()
    {
        if(Microphone.IsRecording(micID))
        {
            Microphone.End(micID);
            Debug.Log("���� ��");

            // ���� ���� �ð��� ���缭 clip ������ �缳��
            AudioClip temp = clip;
            float realLength = clip.length;
            float realSamples = clip.samples;
            float perSec = realSamples / realLength;
            float[] samples = new float[(int)(perSec * recordingLength)];
            temp.GetData(samples, 0);
            clip.SetData(samples, 0);

            if(clip == null)
            {
                Debug.Log("���� �ȵ�");
                return;
            }

			// clip�� byteArray�� ��ȯ
			byte[] byteArray = ClipToByteArray(clip);

			// clip�� stt ������ ������
			StartCoroutine(ToSever(url, byteArray));
		}
    }

    byte[] ClipToByteArray(AudioClip audioClip)
    {
        // �����͸� byteArray�� ������ ���� �޸𸮽�Ʈ�� ���
        MemoryStream memoryStream = new MemoryStream();
        int headerSize = 44;
        ushort bitSize = 16;

        int fileSize = audioClip.samples * blockSize_16bit + headerSize;

		// clip �����͵��� filestream�� �߰�
		WriteFileHeader(ref memoryStream, fileSize);
		WriteFileFormat(ref memoryStream, audioClip.channels, audioClip.frequency, bitSize);
		WriteFileData(ref memoryStream, audioClip, bitSize);

		// filestream�� array ���·� ��ȯ
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

	// ����� ��� ����
	static string lang = "Kor";    // ��� �ڵ� ( Kor, Jpn, Eng, Chn )
	string url = $"https://naveropenapi.apigw.ntruss.com/recog/v1/stt?lang={lang}";

	private IEnumerator ToSever(string url, byte[] data)
	{
		// request ����
		WWWForm form = new WWWForm();
		UnityWebRequest request = UnityWebRequest.Post(url, form);

		// ��û ��� ���� (���̹� Ŭ���� ID, Secret Key)
		request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", "nu4mc31ajs");
		request.SetRequestHeader("X-NCP-APIGW-API-KEY", "GkJTxkR2yrwOz9UDxaXoqqXR9ooxUGCqCrAxHRIH");
		request.SetRequestHeader("Content-Type", "application/octet-stream");

		// data ���
		request.uploadHandler = new UploadHandlerRaw(data);

		// ���� ��û
		yield return request.SendWebRequest();

		// response�� ������ eroor�޼��� ���
		if (request == null)
		{
			Debug.LogError(request.error);
		}
		else
		{
			// json ���·� ���� {"text":"�νİ��"}
			string message = request.downloadHandler.text;
			VoiceRecognize voiceRecognize = JsonUtility.FromJson<VoiceRecognize>(message);

			Debug.Log("Voice Server responded: " + voiceRecognize.text);
			// Voice Server responded: �νİ��
		}
	}
}
