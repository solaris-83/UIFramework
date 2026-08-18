using BaseCustomApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UIFramework.Interfaces;
using UIFramework.Validation;

namespace UIFramework.UIElements
{
    public class UIFileInputBox : UIInputBoxBase, IAttachableContext
    {
        private IUIContext _context;
        private byte[] _contents;
        public UIFileInputBox()
        {
            AddValidationRule(new MinValueValidationRule(1, "No file selected"));
            Description = "BCA_SELECT_FILE";
            FileSize = 0;
            FileName = "";
            FileType = "";
        }

        #region Props

        private bool _acceptImage;
        [JsonIgnore]
        public bool AcceptImage
        {
            get => _acceptImage;
            set => SetPropsProperty(ref _acceptImage, value, nameof(AcceptImage));
        }

        private bool _acceptCertificate;
        [JsonIgnore]
        public bool AcceptCertificate
        {
            get => _acceptCertificate;
            set => SetPropsProperty(ref _acceptCertificate, value, nameof(AcceptCertificate));
        }

        private bool _acceptPdf;
        [JsonIgnore]
        public bool AcceptPdf
        {
            get => _acceptPdf;
            set => SetPropsProperty(ref _acceptPdf, value, nameof(AcceptPdf));
        }

        private bool _acceptTxt;
        [JsonIgnore]
        public bool AcceptTxt
        {
            get => _acceptTxt;
            set => SetPropsProperty(ref _acceptTxt, value, nameof(AcceptTxt));
        }

        private bool _acceptLog;
        [JsonIgnore]
        public bool AcceptLog
        {
            get => _acceptLog;
            set => SetPropsProperty(ref _acceptLog, value, nameof(AcceptLog));
        }

        #endregion

        #region States

        private long _fileSize;
        [JsonIgnore]
        public long FileSize
        {
            get => _fileSize;
            set
            {
                if (_fileSize != value)
                {
                    _fileSize = value;
                    States["fileSize"] = _fileSize;
                    OnPropertyChanged(Id, nameof(FileSize), FileSize);
                    OnPropertyChanged(Id, nameof(HasFile), HasFile);
                }
                ApplyValidationRules(value);
            }
        }

        private DateTime? _lastModifiedDate = null;
        [JsonIgnore]
        public DateTime? LastModifiedDate
        {
            get => _lastModifiedDate;
            set => SetStatesProperty(ref _lastModifiedDate, value, nameof(LastModifiedDate));
        }

        private string _fileType;
        [JsonIgnore]
        public string FileType
        {
            get => _fileType;
            set => SetStatesProperty(ref _fileType, value, nameof(FileType));
        }

        private string _fileName;
        [JsonIgnore]
        public string FileName
        {
            get => _fileName;
            set => SetStatesProperty(ref _fileName, value, nameof(FileName));
        }

        [JsonIgnore]
        public bool HasFile => _fileSize > 0;

        #endregion

        [JsonIgnore]
        public EncryptedData Value => new EncryptedData(_contents);
        public new void AttachContext(IUIContext context)
        {
            _context = context;
            base.AttachContext(context);
        }

        public void OnFileSelected()
        {
            var extensions = "";
            //"Documents & Images (*.pdf;*.log;*.txt;*.jpg;*.jpeg;*.png;*.bmp;*.gif)|" +
            // "*.pdf;*.log;*.txt;*.jpg;*.jpeg;*.png;*.bmp;*.gif|" +
            // "PDF files (*.pdf)|*.pdf|" +
            // "Log files (*.log)|*.log|" +
            // "Text files (*.txt)|*.txt|" +
            // "Image files (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (AcceptCertificate)
            {
                extensions += "Certificate files (*.cer;*.crt;*.pem;*.pfx;*.der;*.key)|*.cer;*.crt;*.pem;*.pfx;*.der;*.key|";
            }
            if (AcceptImage)
            {
                extensions += "Image files (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|";
            }
            if (AcceptLog)
            {
                extensions += "Log files (*.log)|*.log|";
            }
            if (AcceptPdf)
            {
                extensions += "PDF files (*.pdf)|*.pdf|";
            }
            if (AcceptTxt)
            {
                extensions += "Text files (*.txt)|*.txt|";
            }

            var fullPath = _context.FileService.OpenFile(extensions.TrimEnd('|'));
            if (string.IsNullOrEmpty(fullPath))
            {
                FileName = string.Empty;
                FileSize = 0;
                FileType = string.Empty;
                LastModifiedDate = null;
                return;
            }

            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                _contents = new byte[stream.Length];
                stream.Read(_contents, 0, _contents.Length);
            }
            FileName = new FileInfo(fullPath).Name;
            FileSize = new FileInfo(fullPath).Length;
            FileType = new FileInfo(fullPath).Extension;
            LastModifiedDate = new FileInfo(fullPath).LastWriteTime;
        }
    }

    public class FileInputBoxCommand : ICommand
    {
        public UIFileInputBox Value { get; set; }

        public FileInputBoxCommand(UIFileInputBox value)
        {
            Value = value;
        }

        public virtual void Execute(Dictionary<string, object> newStates)
        {
            Value.OnFileSelected();
        }
    }
}
