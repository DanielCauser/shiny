﻿using System;
using System.ComponentModel;
using System.IO;

namespace Shiny.Net.Http
{
    public interface IHttpTransfer : INotifyPropertyChanged
    {
        HttpTransferRequest Request { get; }
        string Identifier { get; }
        bool IsUpload { get; }
        HttpTransferState Status { get; }
        string RemoteFileName { get; }

        long ResumeOffset { get; }
        long FileSize { get; }
        long BytesTransferred { get; }
        decimal PercentComplete { get; }
        double BytesPerSecond { get; }
        TimeSpan EstimatedCompletionTime { get; }
        DateTimeOffset StartTime { get; }

        Exception Exception { get; }

        //void Start(); // or resume
        //void Pause();
        void Cancel();
    }
}