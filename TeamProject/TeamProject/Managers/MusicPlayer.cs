using NAudio.Wave;
using System;
using System.Threading;
using System.Threading.Tasks;

public class MusicPlayer
{
    private WaveOutEvent? waveOutEvent;
    private AudioFileReader? audioFileReader;

    public async Task PlayAsync(string filePath)
    {
        Stop(); // 이미 플레이 중인 경우 중지

        audioFileReader = new AudioFileReader(filePath);
        waveOutEvent = new WaveOutEvent();
        waveOutEvent.Init(audioFileReader);
        waveOutEvent.Play();

        // 비동기로 음악이 끝날 때까지 기다림
        await Task.Run(() =>
        {
            while (waveOutEvent.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(500);
            }
        });
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
}
