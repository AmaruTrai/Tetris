using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Game
{
    /// <summary>
    ///  онтейнер дл€ повторно используемых блоков.
    /// </summary>
    public class BlockPool : MonoBehaviour
    {
        [SerializeField]
        private Block prefab;

        private ObjectPool<Block> pool;

        [Inject]
        private void SetUp(List<Block> blocks)
        {
            pool = new ObjectPool<Block>(
                createFunc: Create,
                actionOnRelease: (obj) => { obj.gameObject.SetActive(false); }
            );

            foreach(var block in blocks)
            {
                block.OnReturn += (obj) => {
                    pool.Release(obj);
                };

                block.Return();
            }
        }

        private Block Create()
        {
            var newBlock = Instantiate(prefab, transform);
            newBlock.gameObject.SetActive(false);
            newBlock.OnReturn += (obj) => {
                pool.Release(obj);
            };
            return newBlock;
        }

        public Block GetBlock()
        {
            return pool.Get();
        }

    }

}