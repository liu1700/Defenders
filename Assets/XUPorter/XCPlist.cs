using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.XCodeEditor 
{
	public class XCPlist
	{
		string plistPath;

		public XCPlist(string plistPath)
		{
			this.plistPath = plistPath;
		}

        public void Process(Hashtable plist)
        {
            if (plist == null || plist.Count == 0) return;

            Dictionary<string, object> dict = (Dictionary<string, object>)PlistCS.Plist.readPlist(plistPath);
            foreach (DictionaryEntry entry in plist)
            {
                object value = PlistItem(entry.Value);
                string key = entry.Key.ToString();
                if (dict.ContainsKey(key))
                {
                    if(dict[key].GetType() == typeof(List<object>) 
                        && value.GetType() == typeof(List<object>))
                    {
                        ((List<object>)dict[key]).AddRange((List<object>)value);
                    }
                    else
					{
						dict[key] = value;
						Debug.LogWarningFormat("[XCPlist] overriding <color=\"red\">{0}</color>", key);
                    }
                }
                else
                {
                    dict[key] = value;
                }
            }
            PlistCS.Plist.writeXml(dict, plistPath);
        }

		// http://stackoverflow.com/questions/20618809/hashtable-to-dictionary
		public static Dictionary<string, object> HashtableToDictionary(Hashtable table)
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			foreach(DictionaryEntry kvp in table)
				dict.Add(kvp.Key.ToString(), PlistItem(kvp.Value));
			return dict;
		}

		public static List<object> ArrayListToList (ArrayList array)
		{
			List<object> list = new List<object>();
			foreach(object v in array)
				list.Add(PlistItem(v));
			return list;
		}
		
		public static object PlistItem(object value)
		{
            if (value.GetType() == typeof(Hashtable))
			{
				return HashtableToDictionary((Hashtable)value);
			}
			else if(value.GetType() == typeof(ArrayList))
			{
				return ArrayListToList((ArrayList)value);
			}
            else // return value
            {
                return value;
            }
		}
	}
}
