using BuildingBlocks.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.PersistMessageProcessor
{
    public class PersistMessage : IVersion
    {
        #region Constructor
        public PersistMessage(Guid id, string dataType, string data, MessageDeliveryType deliveryType)
        {
            Id = id;
            DataType = dataType;
            Data = data;
            DeliveryType = deliveryType;
            Created = DateTime.Now;
            MessageStatus = MessageStatus.InProgress;
            RetryCount = 0;
        }
        #endregion

        #region Properties
        public Guid Id { get; private set; }
        public string DataType { get; private set; }
        public string Data { get; private set; }
        public DateTime Created { get; private set; }
        public int RetryCount { get; private set; }
        public MessageStatus MessageStatus { get; private set; }
        public MessageDeliveryType DeliveryType { get; private set; }
        public long Version { get; set; }
        #endregion

        #region PublicMethods
        public void ChangeState(MessageStatus messageStatus)
        {
            MessageStatus = messageStatus;
        }
        public void IncreaseRetry()
        {
            RetryCount++;
        }
        #endregion
    }
}
