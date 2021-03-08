using System.Collections.Generic;
using System.Linq;

namespace BLL.Helpers
{
    public class SendGridResponse
    {
        public string MessageId { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool Successful => !ErrorMessages.Any();

        public SendGridResponse()
        {
            ErrorMessages = new List<string>();
        }
    }

    public class SendGridResponse<T> : SendGridResponse
    {
        public T Data { get; set; }
    }
}
