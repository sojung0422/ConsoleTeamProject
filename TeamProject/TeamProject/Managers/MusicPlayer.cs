using NAudio.Wave;
using System;
using System.Threading;
using System.Threading.Tasks;

public class MusicPlayer
{
    private static MusicPlayer instance;

    private WaveOutEvent waveOutEvent;
    private AudioFileReader audioFileReader;

    private MusicPlayer() { }

    public static MusicPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MusicPlayer();
            }
            return instance;
        }
    }

    public async Task PlayAsync(string fileName, float volume = 1.0f)
    {
        Stop(); // 이미 플레이 중인 경우 중지

        string filePath = Path.Combine(GetProjectDirectory(), "Sounds", fileName);
        audioFileReader = new AudioFileReader(filePath);
        waveOutEvent = new WaveOutEvent();
        waveOutEvent.Init(audioFileReader);

        // PlaybackStopped 이벤트를 활용하여 음악이 끝났는지 확인
        var playbackStoppedTaskCompletionSource = new TaskCompletionSource<bool>();
        waveOutEvent.PlaybackStopped += (sender, args) =>
        {
            playbackStoppedTaskCompletionSource.TrySetResult(true);
        };
        waveOutEvent.Volume = volume; // 볼륨 조절
        waveOutEvent.Play();

        // 비동기로 음악이 끝날 때까지 기다림
        await playbackStoppedTaskCompletionSource.Task;
    }

    public void Stop()
    {
        if (waveOutEvent != null)
        {
            waveOutEvent.Stop();
            waveOutEvent.Dispose();
            waveOutEvent = null;
        }

        if (audioFileReader != null)
        {
            audioFileReader.Dispose();
            audioFileReader = null;
        }
    }

    private string GetProjectDirectory()
    {
        return Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
    }
}