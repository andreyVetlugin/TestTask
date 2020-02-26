using System;

namespace DataLayer.Entities
{
    public class PostInfoLog: IDbEntity
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public PostOperationType Operation { get; set; }

        public Guid EntityRootId { get; set; }

    }


    public enum PostOperationType : byte
    {
        
    }

}
