/**
 * DynamicScrollViewItemExample.cs
 * 
 * @author mosframe / https://github.com/mosframe
 * 
 */

namespace Mosframe
{

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ItemListScrollView : UIBehaviour, IDynamicScrollViewItem
    {
        ItemsDataCacher cacher;

        public Text desc, cost;
        public Image icon;

        protected override void Awake()
        {
            cacher = GameObject.Find("ItemData").GetComponent<ItemsDataCacher>();
        }

        public void onUpdateItem(int index)
        {
            //this.desc.text = string.Format("Name{0:d3}", (index + 1));
            //this.icon.color = this.colors[Mathf.Abs(index) % this.colors.Length];
            var name = cacher.sprites[index].name;
            this.desc.text = cacher.spriteDesc[name];
            this.icon.sprite = cacher.sprites[index];
            this.cost.text = cacher.spriteCost[name].ToString();
        }
    }
}