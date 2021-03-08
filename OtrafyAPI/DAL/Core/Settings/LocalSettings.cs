namespace DAL.Core.Settings
{
    public class LocalSettings
    {
        public LocalSettings()
        {
            SendGridConfig = new SendGridConfig();
            AzureBlobStorageConfig = new AzureBlobStorageConfig();
            MongodbConnectionStrings = new MongodbConnectionStrings();
        }
        public SendGridConfig SendGridConfig { get; set; }
        public AzureBlobStorageConfig AzureBlobStorageConfig { get; set; }
        public MongodbConnectionStrings MongodbConnectionStrings { get; set; }
    }
}
