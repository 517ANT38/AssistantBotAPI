namespace AssistantBotAPI.JobWithData
{
    public class SaveCommandAndDat
    {
        public string Name { get; set; }
        public long chatID { get; set; }
        public string Dat { get; set; }
        public SaveCommandAndDat(string strName, long chatId,string date)
        {
            this.Name = strName;
            this.chatID = chatId;   
            this.Dat = date;
        }
        
    }
}
