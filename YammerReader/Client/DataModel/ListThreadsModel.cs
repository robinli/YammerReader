using YammerReader.Shared;

namespace YammerReader.Client.DataModel
{
    public class ListThreadsModel
    {
        public YammerGroup? Group { get; set; } = new YammerGroup();
        
        public List<YammerMessage>? ListData { get; set; }

        public PagerModel Pager = new PagerModel();
    }
}
