using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Anthyme.LongPollingChat.Models;

namespace Anthyme.LongPollingChat
{
    [ServiceContract]
    public interface IChatService
    {
        [OperationContract, WebInvoke(ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        CreateSessionResult CreateSession(CreateSessionParameter parameter);

        [OperationContract, WebInvoke(ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetUpdatesResult GetUpdates(GetUpdatesParameter parameter);

        [OperationContract, WebInvoke(ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        SendMessageResult SendMessage(SendMessageParameter parameter);
    }
}
