using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class EntityManager
    {
        Maincode mc;
        public EntityManager(Maincode parent)
        {
            mc = parent;

            addBuffer = new List<AbstrEntity> ();
        }

        public List<AbstrEntity> addBuffer;

        public void ProccessAdd()
        {
            while (addBuffer.Count > 0)
            {
                var entitiesToAddThisIteration = new List<AbstrEntity>(addBuffer);
                addBuffer.Clear();

                foreach (AbstrEntity item in entitiesToAddThisIteration)
                {
                    mc.entities.Add(item);
                    item.OnSpawn();
                }
            }
        }

        public void ProcessRem()
        {
            var entitiesToRemove = new List<AbstrEntity>();

            foreach (AbstrEntity item in mc.entities)
            {
                if (item.markForDestroy)
                {
                    entitiesToRemove.Add(item);
                }
            }

            

            foreach (AbstrEntity item in entitiesToRemove)
            {
                mc.entities.Remove(item);
                item.OnDestroy();
            }
        }

        public void Apply()
        {
            ProccessAdd();
            ProcessRem();
        }

        public void AddEntity(AbstrEntity entity)
        {
            entity.isCreated = true;

            addBuffer.Add(entity);
        }
    }
}
