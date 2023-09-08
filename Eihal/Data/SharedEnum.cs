namespace Eihal.Data
{
    public class SharedEnum
    {
        public enum AccountTypeEnum
        {
            Admin = 0,
            ServiceProvider = 1,
            Beneficiary = 2
        }

        public enum NotificationTypeEnum
        {
            NewOrder = 1,
            ApprovedOrder = 2,
            RejectOrder = 3,
            SendProfileToReview = 4,
            RejectProfile = 5,
            SendNewService = 6,
            ApprovedNewService = 7,
            RejectNewService = 8,

        }



    }
}
