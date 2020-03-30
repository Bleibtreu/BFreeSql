using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseFreeSql.Entity
{
    class OneToMany
    {
    }
    /*
    class Group
    {
        public int Id { get; set; } //Id、GroupId、Group_id

        public ICollection<User> AUsers { get; set; }
        public ICollection<User> BUsers { get; set; }
    }

    class User
    {
        public int Id { get; set; } //Id、UserId、User_id


        public int AGroupId { get; set; }
        public Group AGroup { get; set; }

        public int BGroupId { get; set; }
        public Group BGroup { get; set; }
    }
    */
    /*
    class Tag
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? Parent_id { get; set; }
        public virtual Tag Parent { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
    */
    
}
