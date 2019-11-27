using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Super.Core.Infrastruct.EF
{
    public interface IEntity
    {

    }

    public interface IEntity<TPrimaryKey> : IEntity
    {
        [Key]
        TPrimaryKey Id { get; set; }
    }
}
