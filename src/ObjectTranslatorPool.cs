using System;
using System.Collections;
using System.Collections.Generic;
using LuaState = KeraLua.Lua;

namespace NLua
{

    internal class ObjectTranslatorPool
    {
        private static volatile ObjectTranslatorPool _instance = new ObjectTranslatorPool();

        private Dictionary<LuaState, ObjectTranslator> translators = new Dictionary<LuaState, ObjectTranslator>();

        public static ObjectTranslatorPool Instance => _instance;


        public void Add(LuaState luaState, ObjectTranslator translator)
        {
            lock (translators)
            {
                try
                {
                    translators.Add(luaState, translator);
                }
                catch (Exception)
                {
                    throw new ArgumentException("An item with the same key has already been added. ", "luaState");
                }
            }
        }

        public ObjectTranslator Find(LuaState luaState)
        {
            ObjectTranslator translator;

            if(!translators.TryGetValue(luaState, out translator))
            {
                LuaState main = luaState.MainThread;

                if (!translators.TryGetValue(main, out translator))
                    return null;
            }
            return translator;
        }

        public void Remove(LuaState luaState)
        {
            lock (translators)
                translators.Remove(luaState);
        }
    }
}

