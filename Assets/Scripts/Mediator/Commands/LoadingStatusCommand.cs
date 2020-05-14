using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vevidi.FindDiff.GameMediator
{
    // TODO: add loading status, etc here
    public class LoadingStatusCommand : ICommand
    {
        public enum eLoadingStatus
        {
            Ok,
            Error
        }

        public string message;
        public eLoadingStatus status;

        public LoadingStatusCommand(eLoadingStatus status, string message)
        {
            this.message = message;
            this.status = status;
        }
    }
}