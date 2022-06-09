namespace YammerReader.Client.DataModel
{
    public class PagerDto
    {
        public PagerDto()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        /// <summary>
        /// 總比數
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage
        {
            get
            {
                return (AllCount / PageSize) + (AllCount % PageSize > 0 ? 1 : 0);
            }
        }
    }
}
