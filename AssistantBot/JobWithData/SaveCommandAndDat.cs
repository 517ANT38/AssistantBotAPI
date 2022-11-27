namespace AssistantBotAPI.JobWithData
{
    public struct SaveCommandAndDat
    {
        public string Name { get; }
        public long chatID { get; }
        public string Dat { get; }
        public SaveCommandAndDat(string strName, long chatId,string date)
        {
            this.Name = strName;
            this.chatID = chatId;   
            this.Dat = date;
        }
        
    }
}
