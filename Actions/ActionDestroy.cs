using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionDestroy : AbstrAction, IUseCloneFollows
    {
        public List<AbstrEntity> follows { get; set; } = new List<AbstrEntity>();

        public override void Act()
        {
            foreach (AbstrEntity item in follows)
            {
                if (item != null) item.Destroy();
            }
        }

        public void NewClone(AbstrEntity clone)
        {
            follows.Add(clone);
        }

        public ActionDestroy(Maincode imc, AbstrEntity entity = null) : base(imc)
        {
            entity.useCloneFollows.Add(this);

            follows.Add(entity);
        }
    }
}