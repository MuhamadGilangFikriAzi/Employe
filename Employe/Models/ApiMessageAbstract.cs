namespace EmployeAPI.Models
{
    public abstract class ApiMessageAbstract
    {
        private string strTransactionGUID;
        public string ModuleName { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public List<AdditionalMessage> Message { get; set; }
        public ApiMessageAbstract()
        {
            TransactionGUID = Guid.NewGuid().ToString().ToUpper();
            TransactionDateTime = DateTime.Now;
            Message = new List<AdditionalMessage>();
        }
        public string TransactionGUID
        {
            get
            {
                return strTransactionGUID;
            }
            set
            {
                strTransactionGUID = value?.ToUpper();
            }
        }
        public void CopyHeader(ApiMessageAbstract apiMessage)
        {
            TransactionGUID = apiMessage.TransactionGUID;
            ModuleName = apiMessage.ModuleName;
        }
    }

    public class AdditionalMessage
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
    }
}
