using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DevourDev.Unity.Utils
{
    public static class EditorHelpers
    {
#if UNITY_EDITOR
        private static readonly Type[] _typesBuffer = new Type[1024 * 8];
        private static readonly ReadOnlyMemory<Type> _typesMemBuffer = new(_typesBuffer);


        public static string AbsToRelativePath(string absPath)
        {
            return absPath[absPath.LastIndexOf("Assets")..];
        }

        public static string DirectoryUp(string path)
        {
            int last = path.LastIndexOf('/');

            if (last < 0)
                last = path.LastIndexOf(Path.DirectorySeparatorChar);

            ++last;

            return path[..last];
        }

        public static ReadOnlyMemory<Type> GetInheritedTypes(Type type)
        {
            return GetInheritedTypes(type, true);
        }

        public static ReadOnlyMemory<Type> GetInheritedTypes(Type type, bool includeBase)
        {
            return GetInheritedTypes(type, includeBase, AppDomain.CurrentDomain.GetAssemblies());
        }

        public static ReadOnlyMemory<Type> GetInheritedTypes(Type type, bool includeBase, Assembly[] assemblies)
        {
            int typesCount = 0;

            if (includeBase)
                _typesBuffer[typesCount++] = type;

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (var t in types)
                {
                    if (t.IsSubclassOf(type))
                        _typesBuffer[typesCount++] = t;
                }
            }

            return _typesMemBuffer[..typesCount];
        }


        public static string[] GetAssetsOfTypeGuids<T>()
        {
            return GetAssetsOfTypeGuids(typeof(T));
        }

        public static string[] GetAssetsOfTypeGuids<T>(string path)
        {
            return GetAssetsOfTypeGuids(typeof(T), path);
        }

        public static string[] GetAssetsOfTypeGuids(Type type)
        {
            return UnityEditor.AssetDatabase.FindAssets($"t:{type.Name}");
        }

        public static string[] GetAssetsOfTypeGuids(Type type, string path)
        {
            return UnityEditor.AssetDatabase.FindAssets($"t:{type.Name}", new string[] { path });
        }


        public static T GetFirstAssetOfType<T>() where T : UnityEngine.Object
        {
            string[] guids = GetAssetsOfTypeGuids<T>();
            return GuidToAsset<T>(guids[0]);
        }

        public static int FindAssetsOfTypeToCollection<T>(ICollection<T> collection) where T : UnityEngine.Object
        {
            string[] guids = GetAssetsOfTypeGuids<T>();

            foreach (var guid in guids)
                collection.Add(GuidToAsset<T>(guid));

            return guids.Length;
        }

        public static int FindAssetsOfType(Type type, ICollection<UnityEngine.Object> collection)
        {
            string[] guids = GetAssetsOfTypeGuids(type);

            foreach (var guid in guids)
                collection.Add(GuidToAsset(guid, type));

            return guids.Length;
        }


        public static int FindAssetsOfTypeAtPath(Type type, ICollection<UnityEngine.Object> collection, string path)
        {
            string[] guids = GetAssetsOfTypeGuids(type, path);

            foreach (var guid in guids)
                collection.Add(GuidToAsset(guid, type));

            return guids.Length;
        }

        public static int GetAssetsAtPath(ICollection<UnityEngine.Object> collection, string path)
        {
            return FindAssetsOfTypeAtPath(typeof(UnityEngine.Object), collection, path);
        }

        public static int FindAssetsOfTypeAtPath<T>(ICollection<T> collection, string rootPath) where T : UnityEngine.Object
        {
            int count = 0;

            string searchFilter = $"t:{typeof(T).Name}";
            string[] guids = UnityEditor.AssetDatabase.FindAssets(searchFilter, new string[] { rootPath });

            foreach (var guid in guids)
            {
                collection.Add(GuidToAsset<T>(guid));
                ++count;
            }

            return count;
        }

        public static TAsset GuidToAsset<TAsset>(string guid) where TAsset : UnityEngine.Object
        {
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            return (TAsset)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(TAsset));
        }

        public static UnityEngine.Object GuidToAsset(string guid, Type type)
        {
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            return UnityEditor.AssetDatabase.LoadAssetAtPath(path, type);
        }


        /// <summary>
        /// hashset as collection recommended
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static int FindAssetsOfTypeIncludingSubclasses<T>(ICollection<T> collection) where T : UnityEngine.Object
        {
            int count = 0;

            var types = GetInheritedTypes(typeof(T), true).Span;

            foreach (var t in types)
            {
                var findAssetsGenericMethod = typeof(EditorHelpers).GetMethod(nameof(FindAssetsOfTypeToCollection),
                    BindingFlags.Static | BindingFlags.Public)
                    .MakeGenericMethod(new Type[] { t });

                count += (int)findAssetsGenericMethod.Invoke(null, new object[] { collection });
            }

            return count;
        }

        public static HashSet<UnityEngine.Object> FindAssetsOfTypeIncludingSubclasses<T>(string path)
            where T : UnityEngine.Object
        {
            int count = 0;

            var types = GetInheritedTypes(typeof(T), true).Span;
            var hs = new HashSet<UnityEngine.Object>();

            foreach (var t in types)
            {
                count += FindAssetsOfTypeAtPath(t, hs, path);
            }

            return hs;
        }
#endif
    }
}
