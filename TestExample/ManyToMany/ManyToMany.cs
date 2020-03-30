using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql.Entity
{
    class ManyToMany
    {
    }
    
    class Song
    {
        [Column(IsIdentity = true, IsPrimary =true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
    class Song_tag
    {
        public int Song_id { get; set; }
        public virtual Song Song { get; set; }

        public int Tag_id { get; set; }
        public virtual Tag Tag { get; set; }
    }
    
    class Tag
    {
        [Column(IsIdentity = true, IsPrimary =true)]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? Parent_id { get; set; }
        public virtual Tag Parent { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
    
    
    /*
    [Table(Name = "EAUNL_MTM_SONG")]
    class Song
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Tag> Tags { get; set; }
    }
    [Table(Name = "EAUNL_MTM_TAG")]
    class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Song> Songs { get; set; }
    }
    [Table(Name = "EAUNL_MTM_SONGTAG")]
    class SongTag
    {
        public Guid SongId { get; set; }
        public Song Song { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
    */
}
