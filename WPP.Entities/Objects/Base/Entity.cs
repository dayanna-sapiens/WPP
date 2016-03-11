using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Entities.Objects.Base
{
    public abstract class Entity
    {
        public virtual long Id { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual int Version { get; set; }
        public virtual DateTime? DateLastModified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? DeleteDate { get; set; }
        public virtual string DeletedBy { get; set; }
    }
}
