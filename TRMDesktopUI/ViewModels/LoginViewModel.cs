﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.Api;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
		private string _userName = "tim@iamtimcorey.com";
		private string _password = "Pwd12345.";
		private string _errorMessage;
		private IAPIHelper _aPIHelper;
		private IEventAggregator _events;

        #region Public Properties

        public string UserName
		{
			get { return _userName; }
			set 
			{ 
				_userName = value;
				NotifyOfPropertyChange(() => UserName);
				NotifyOfPropertyChange(() => CanLogIn);
			}
		}

		public string Password
		{
			get { return _password; }
			set 
			{ 
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogIn);
			}
		}

        #endregion

        public bool IsErrorVisible
		{
			get 
			{
				bool output = false;
				if (ErrorMessage?.Length > 0)
				{
					output = true;
				}
				return output;
			}
		}

		public string ErrorMessage
		{
			get { return _errorMessage; }
			set
			{ 
				_errorMessage = value;
				NotifyOfPropertyChange(() => IsErrorVisible);
				NotifyOfPropertyChange(() => ErrorMessage);
			}
		}

		public LoginViewModel(IAPIHelper aPIHelper, IEventAggregator events)
		{
			_aPIHelper = aPIHelper;
			_events = events;
		}

		public bool CanLogIn
		{
			get
			{
				bool output = false;
				if (UserName?.Length > 0 && Password?.Length > 0)
				{
					output = true;
				}

				return output;
			}
			
		}

		public async Task LogIn()
		{
			try
			{
				ErrorMessage = "";
				var result = await _aPIHelper.Authenticate(UserName, Password);
				await _aPIHelper.GetLoggedInUserInfo(result.Access_Token);

				_events.PublishOnUIThread(new LogOnEvent());
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}
		}
	}
}
