/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dev2;
using Dev2.Common.ExtMethods;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Threading;
using Dev2.Common.Interfaces.ToolBase.Email;
using Dev2.Runtime.Configuration.ViewModels.Base;
using Dev2.Studio.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Warewolf.Studio.ViewModels
{
    public class ManageEmailSourceViewModel : SourceBaseImpl<ISmtpSource>, IManageEmailSourceViewModel, IDataErrorInfo
    {
        string _hostName;
        string _userName;
        string _password;
        int _port;
        int _timeout;
        string _testMessage;
        string _emailFrom;
        string _emailTo;
        string _resourceName;
        bool _enableSsl;
        bool _enableSslYes;
        bool _enableSslNo;

        ISmtpSource _emailServiceSource;
        readonly IManageEmailSourceModel _updateManager;
        CancellationTokenSource _token;
        bool _testPassed;
        bool _testFailed;
        bool _testing;
        string _headerText;
        bool _enableSend;
        bool _isDisposed;

        public ManageEmailSourceViewModel(IManageEmailSourceModel updateManager, Task<IRequestServiceNameViewModel> requestServiceNameViewModel, IEventAggregator aggregator)
            : this(updateManager, aggregator)
        {
            VerifyArgument.IsNotNull("requestServiceNameViewModel", requestServiceNameViewModel);
            _updateManager = updateManager;
            RequestServiceNameViewModel = requestServiceNameViewModel;
            HeaderText = Resources.Languages.Core.EmailSourceNewHeaderLabel;
            Header = Resources.Languages.Core.EmailSourceNewHeaderLabel;
            HostName = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            EnableSend = false;
            EnableSslNo = true;
            Port = 25;
            Timeout = 10000;
        }

        public ManageEmailSourceViewModel(IManageEmailSourceModel updateManager, IEventAggregator aggregator, ISmtpSource emailServiceSource, IAsyncWorker asyncWorker)
            : this(updateManager, aggregator)
        {
            VerifyArgument.IsNotNull("emailServiceSource", emailServiceSource);
            asyncWorker.Start(() => updateManager.FetchSource(emailServiceSource.ResourceID), source =>
            {
                _emailServiceSource = source;
                _emailServiceSource.Path = emailServiceSource.Path;
                FromModel(_emailServiceSource);
                Item = ToModel();
                SetupHeaderTextFromExisting();
            });
        }

        public ManageEmailSourceViewModel(IManageEmailSourceModel updateManager, IEventAggregator aggregator)
            : base("EmailSource")
        {
            VerifyArgument.IsNotNull("updateManager", updateManager);
            VerifyArgument.IsNotNull("aggregator", aggregator);
            _updateManager = updateManager;
            SendCommand = new DelegateCommand(o => TestConnection(), o => CanTest());
            OkCommand = new DelegateCommand(o => SaveConnection(), o => CanSave());
            Testing = false;
            _testPassed = false;
            _testFailed = false;
        }

        public ManageEmailSourceViewModel()
            : base("EmailSource")
        {
        }

        public override string Name
        {
            get => ResourceName;
            set
            {
                ResourceName = value;
            }
        }

        public override void FromModel(ISmtpSource service)
        {
            if (service != null)
            {
                HostName = service.Host;
                UserName = service.UserName;
                Password = service.Password;
                EnableSsl = service.EnableSSL;
                if (EnableSsl)
                {
                    EnableSslYes = EnableSsl;
                }
                else
                {
                    EnableSslNo = true;
                }
                Port = service.Port;
                Timeout = service.Timeout;
                EmailFrom = service.EmailFrom;
                EmailTo = service.EmailTo;
                ResourceName = service.ResourceName;
            }
        }

        void SetupHeaderTextFromExisting()
        {
            if (_emailServiceSource != null)
            {
                HeaderText = (_emailServiceSource.ResourceName ?? ResourceName).Trim();
                Header = (_emailServiceSource.ResourceName ?? ResourceName).Trim();
            }
        }

        public override bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(HostName);
        }

        public bool CanTest()
        {
            if (Testing)
            {
                return false;
            }

            if (string.IsNullOrEmpty(HostName) && string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Password))
            {
                return false;
            }
            return true;
        }

        public override void UpdateHelpDescriptor(string helpText)
        {
            var mainViewModel = CustomContainer.Get<IShellViewModel>();
            mainViewModel?.HelpViewModel.UpdateHelpText(helpText);
        }

        public override void Save()
        {
            SaveConnection();
        }

        public string ResourceName
        {
            get => _resourceName;
            set
            {
                _resourceName = value;
                if (!string.IsNullOrEmpty(value))
                {
                    SetupHeaderTextFromExisting();
                }
                OnPropertyChanged(_resourceName);
            }
        }

        void SaveConnection()
        {
            if (_emailServiceSource == null)
            {
                RequestServiceNameViewModel.Wait();
                if (RequestServiceNameViewModel.Exception == null)
                {
                    var requestServiceNameViewModel = RequestServiceNameViewModel.Result;
                    var res = requestServiceNameViewModel.ShowSaveDialog();

                    if (res == MessageBoxResult.OK)
                    {
                        var src = ToSource();
                        src.ResourceName = requestServiceNameViewModel.ResourceName.Name;
                        src.Path = requestServiceNameViewModel.ResourceName.Path ?? requestServiceNameViewModel.ResourceName.Name;
                        Save(src);
                        if (requestServiceNameViewModel.SingleEnvironmentExplorerViewModel != null)
                        {
                            AfterSave(requestServiceNameViewModel.SingleEnvironmentExplorerViewModel.Environments[0].ResourceId, src.ResourceID);
                        }

                        Item = src;
                        _emailServiceSource = src;
                        ResourceName = _emailServiceSource.ResourceName;
                        SetupHeaderTextFromExisting();
                    }
                }
            }
            else
            {
                var src = ToSource();
                Save(src);
                Item = src;
                _emailServiceSource = src;
                SetupHeaderTextFromExisting();
            }
            TestPassed = false;
        }

        public Task<IRequestServiceNameViewModel> RequestServiceNameViewModel { get; set; }

        void Save(ISmtpSource source)
        {
            _updateManager.Save(source);
        }

        public string HostName
        {
            get => _hostName;
            set
            {
                if (value != _hostName)
                {
                    _hostName = value;
                    TestMessage = string.Empty;

                    OnPropertyChanged(() => HostName);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => TestMessage);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    EmailFrom = _userName;
                    TestMessage = string.Empty;

                    OnPropertyChanged(() => UserName);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => TestMessage);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {
                    _password = value;
                    TestMessage = string.Empty;

                    OnPropertyChanged(() => Password);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => TestMessage);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public bool EnableSsl
        {
            get => _enableSsl;
            set
            {
                if (value != _enableSsl)
                {
                    _enableSsl = value;
                    TestMessage = string.Empty;

                    OnPropertyChanged(() => EnableSsl);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => TestMessage);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }
        public bool EnableSslYes
        {
            get => _enableSslYes;
            set
            {
                _enableSslYes = value;
                EnableSsl |= _enableSslYes;

                OnPropertyChanged(() => EnableSslYes);
                OnPropertyChanged(() => EnableSsl);
                OnPropertyChanged(() => Header);
                TestPassed = false;
                ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
            }
        }
        public bool EnableSslNo
        {
            get => _enableSslNo;
            set
            {
                _enableSslNo = value;
                EnableSsl &= !_enableSslNo;

                OnPropertyChanged(() => EnableSslNo);
                OnPropertyChanged(() => EnableSsl);
                OnPropertyChanged(() => Header);
                TestPassed = false;
                ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                if (value != _port)
                {
                    _port = value;
                    TestMessage = string.Empty;

                    if (!_port.ToString().IsNumeric())
                    {
                        OkCommand.CanExecute(false);
                    }

                    OnPropertyChanged(() => Port);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => TestMessage);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public int Timeout
        {
            get => _timeout;
            set
            {
                if (value != _timeout)
                {
                    _timeout = value;
                    TestMessage = string.Empty;

                    if (!_timeout.ToString().IsNumeric())
                    {
                        OkCommand.CanExecute(false);
                    }

                    OnPropertyChanged(() => Timeout);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => TestMessage);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public string EmailFrom
        {
            get => _emailFrom;
            set
            {
                if (value != _emailFrom)
                {
                    _emailFrom = value;
                    TestMessage = string.Empty;

                    EnableSend = true;
                    EnableSend &= _emailFrom.IsEmail();
                    EnableSend &= (EmailTo != null && EmailTo.IsEmail());

                    OnPropertyChanged(() => EmailFrom);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => EnableSend);
                    OnPropertyChanged(() => TestMessage);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public string EmailTo
        {
            get => _emailTo;
            set
            {
                if (value != _emailTo)
                {
                    _emailTo = value;
                    TestMessage = string.Empty;

                    EnableSend = true;
                    EnableSend &= _emailTo.IsEmail();
                    EnableSend &= (EmailFrom != null && EmailFrom.IsEmail());

                    OnPropertyChanged(() => EmailTo);
                    OnPropertyChanged(() => Header);
                    OnPropertyChanged(() => EnableSend);
                    OnPropertyChanged(() => TestMessage);
                    TestPassed = false;
                    ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
                    ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
                }
            }
        }

        public bool TestPassed
        {
            get => _testPassed;
            set
            {
                _testPassed = value;
                OnPropertyChanged(() => TestPassed);
                ViewModelUtils.RaiseCanExecuteChanged(OkCommand);
            }
        }

        public string HeaderText
        {
            get => _headerText;
            set
            {
                _headerText = value;
                OnPropertyChanged(() => HeaderText);
                OnPropertyChanged(() => Header);
            }
        }

        void TestConnection()
        {
            _token = new CancellationTokenSource();
            var t = new Task(SetupProgressSpinner, _token.Token);

            t.ContinueWith(a => Application.Current?.Dispatcher?.Invoke(() =>
            {
                if (!_token.IsCancellationRequested)
                {
                    switch (t.Status)
                    {
                        case TaskStatus.Faulted:
                            {
                                TestFailed = true;
                                TestPassed = false;
                                Testing = false;
                                TestMessage = GetExceptionMessage(t.Exception);
                                break;
                            }
                        case TaskStatus.RanToCompletion:
                            {
                                TestMessage = "Passed";
                                TestFailed = false;
                                Testing = false;
                                TestPassed = true;
                                break;
                            }

                        case TaskStatus.Created:
                            break;
                        case TaskStatus.WaitingForActivation:
                            break;
                        case TaskStatus.WaitingToRun:
                            break;
                        case TaskStatus.Running:
                            break;
                        case TaskStatus.WaitingForChildrenToComplete:
                            break;
                        case TaskStatus.Canceled:
                            break;
                        default:
                            break;
                    }
                }
            }));
            t.Start();
        }

        void SetupProgressSpinner()
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                Testing = true;
                TestFailed = false;
                TestPassed = false;
            });
            _updateManager.TestConnection(ToNewSource());
        }

        ISmtpSource ToNewSource()
        {
            var resourceID = _emailServiceSource == null ? Guid.NewGuid() : _emailServiceSource.ResourceID;
            return new EmailServiceSourceDefinition
            {
                Host = HostName,
                Password = Password,
                UserName = UserName,
                Port = Port,
                Timeout = Timeout,
                EnableSSL = EnableSsl,
                EmailFrom = EmailFrom,
                EmailTo = EmailTo,
                ResourceName = Name,
                ResourceType = "EmailSource",
                Id = resourceID,
                ResourceID = resourceID
            };
        }

        ISmtpSource ToSource()
        {
            if (_emailServiceSource == null)
            {
                var resourceID = Guid.NewGuid();
                return new EmailServiceSourceDefinition
                {
                    Host = HostName,
                    Password = Password,
                    UserName = UserName,
                    Port = Port,
                    Timeout = Timeout,
                    EnableSSL = EnableSsl,
                    EmailFrom = EmailFrom,
                    EmailTo = EmailTo,
                    ResourceName = ResourceName,
                    ResourceType = "EmailSource",
                    Id = resourceID,
                    ResourceID = resourceID
                };
            }
            _emailServiceSource.Host = HostName;
            _emailServiceSource.UserName = UserName;
            _emailServiceSource.Password = Password;
            _emailServiceSource.Port = Port;
            _emailServiceSource.Timeout = Timeout;
            _emailServiceSource.EnableSSL = EnableSsl;
            _emailServiceSource.EmailFrom = EmailFrom;
            _emailServiceSource.EmailTo = EmailTo;
            return _emailServiceSource;
        }

        public override ISmtpSource ToModel()
        {
            if (Item == null)
            {
                Item = ToSource();
                return Item;
            }
            var resourceID = _emailServiceSource == null ? Guid.NewGuid() : _emailServiceSource.ResourceID;
            return new EmailServiceSourceDefinition
            {
                Host = HostName,
                Password = Password,
                UserName = UserName,
                Port = Port,
                Timeout = Timeout,
                EnableSSL = EnableSsl,
                EmailFrom = EmailFrom,
                EmailTo = EmailTo,
                ResourceType = "EmailSource",
                ResourceName = ResourceName,
                Id = resourceID,
                ResourceID = resourceID
            };
        }

        public bool TestFailed
        {
            get => _testFailed;
            set
            {
                _testFailed = value;
                OnPropertyChanged(() => TestFailed);
            }
        }
        public bool Testing
        {
            get => _testing;
            private set
            {
                _testing = value;
                OnPropertyChanged(() => Testing);
                ViewModelUtils.RaiseCanExecuteChanged(SendCommand);
            }
        }

        public string TestMessage
        {
            get => _testMessage;
            set
            {
                _testMessage = value;
                OnPropertyChanged(() => TestMessage);
                OnPropertyChanged(() => TestPassed);
            }
        }

        public ICommand SendCommand { get; set; }
        public ICommand OkCommand { get; set; }

        public bool EnableSend
        {
            get => _enableSend;
            set
            {
                _enableSend = value;
                OnPropertyChanged(() => EnableSend);
            }
        }

        public string HostNameLabel => Resources.Languages.Core.HostNameLabel;

        public string UserNameLabel => Resources.Languages.Core.UserNameLabel;

        public string PasswordLabel => Resources.Languages.Core.PasswordLabel;

        public string EnableSslLabel => Resources.Languages.Core.EmailSourceEnableSslLabel;

        public string PortLabel => Resources.Languages.Core.PortLabel;

        public string TimeoutLabel => Resources.Languages.Core.EmailSourceTimeoutLabel;

        public string TestLabel => Resources.Languages.Core.TestConnectionLabel;

        public string EmailFromLabel => Resources.Languages.Core.EmailSourceEmailFromLabel;

        public string EmailToLabel => Resources.Languages.Core.EmailSourceEmailToLabel;

        protected override void OnDispose()
        {
            if (RequestServiceNameViewModel != null)
            {
                RequestServiceNameViewModel.Result?.Dispose();
                RequestServiceNameViewModel.Dispose();
            }
            DisposeManageEmailSourceViewModel(true);
        }

        void DisposeManageEmailSourceViewModel(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _token?.Dispose();
                }
                _isDisposed = true;
            }
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                var errorMessage = string.Empty;
                switch (columnName)
                {
                    case "HostName":
                        if (string.IsNullOrEmpty(HostName))
                        {
                            errorMessage = "HostName cannot be blank.";
                        }
                        break;
                    case "Port":
                        if (string.IsNullOrEmpty(Port.ToString()) || Port == 0)
                        {
                            errorMessage = "Port cannot be blank.";
                        }
                        if (Port < 1 || Port > 65535)
                        {
                            errorMessage = "Port range must be between 1 and 65535.";
                        }
                        break;
                    case "Timeout":
                        if (string.IsNullOrEmpty(Timeout.ToString()) || Timeout == 0)
                        {
                            errorMessage = "Timeout cannot be blank.";
                        }
                        break;
                }
                return errorMessage;
            }
        }
    }
}
