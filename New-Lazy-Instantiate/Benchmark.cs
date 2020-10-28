using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System.Collections.Generic;

namespace New_Lazy_Instantiate
{
    //[ShortRunJob]
    [SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    public class Benchmark
    {
        private const int count = 10;

        [Benchmark(Description = "10 times Ctor Instantiate")]
        public void Ctor_Instantiate()
        {
            for (int i = 0; i < count; i++)
            {
                new Article_CtorInstantiate();
            }
        }

        [Benchmark(Baseline = true, Description = "10 times Lazy Instantiate")]
        public void Lazy_Instantiate()
        {
            for (int i = 0; i < count; i++)
            {
                new Article_LazyInstantiate();
            }
        }
    }

    #region Models
    public class Article_CtorInstantiate
    {
        public Article_CtorInstantiate()
        {
            Categories = new List<Category>();
            Tags = new List<Tag>();
            Comments = new List<Comment>();
            Attachments = new List<Attachment>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    public class Article_LazyInstantiate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        private List<Category> categories;
        private List<Tag> tags;
        private List<Comment> comments;
        private List<Attachment> attachments;

        public List<Category> Categories { get => categories ??= new List<Category>(); set => categories = value; }
        public List<Tag> Tags { get => tags ??= new List<Tag>(); set => tags = value; }
        public List<Comment> Comments { get => comments ??= new List<Comment>(); set => comments = value; }
        public List<Attachment> Attachments { get => attachments ??= new List<Attachment>(); set => attachments = value; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Attachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion
}
