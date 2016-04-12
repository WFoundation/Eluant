//
// LuaApi.cs
//
// Authors:
//       Chris Howie <me@chrishowie.com>
//       Dirk Weltz <web@weltz-online.de>
//       Brice Clocher <contact@cybisoft.net>
//
// Copyright (c) 2013 Chris Howie
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Runtime.InteropServices;
using System.Text;

#if USE_KOPILUA
using KopiLua;
#endif

namespace Eluant
{    
    internal static class LuaApi
    {
        public enum LuaType : int
        {
            None = -1,

            Nil = 0,
            Boolean = 1,
            LightUserdata = 2,
            Number = 3,
            String = 4,
            Table = 5,
            Function = 6,
            Userdata = 7,
            Thread = 8,
        }

        public enum LuaGcOperation : int
        {
            Stop = 0,
            Restart = 1,
            Collect = 2,
            Count = 3,
            Countb = 4,
            Step = 5,
            SetPause = 6,
            SetStepMul = 7,
        }

#if USE_KOPILUA

        public static int LUA_REGISTRYINDEX { get { return KopiLua.Lua.LUA_REGISTRYINDEX; } }

        public static int LUA_MULTRET { get { return KopiLua.Lua.LUA_MULTRET; } }

        public static int LUA_GLOBALSINDEX { get { return KopiLua.Lua.LUA_GLOBALSINDEX; } }

        internal static KopiLua.LuaState luaL_newstate()
        {
            return Lua.LuaLNewState();
        }

        internal static void lua_getfield(KopiLua.LuaState state, int idx, string k)
        {
            Lua.LuaGetField(state, idx, k);
        }

        internal static object lua_touserdata(KopiLua.LuaState state, int idx)
        {
            return Lua.LuaToUserData(state, idx);
        }

        internal static void lua_pop(KopiLua.LuaState state, int n)
        {
            Lua.LuaPop(state, n);
        }

        internal static void lua_newtable(KopiLua.LuaState state)
        {
            Lua.LuaNewTable(state);
        }

        internal static void lua_setfield(KopiLua.LuaState state, int idx, string k)
        {
            Lua.LuaSetField(state, idx, k);
        }

        internal static void luaL_openlibs(KopiLua.LuaState state)
        {
            Lua.LuaLOpenLibs(state);
        }

        internal static void lua_pushlightuserdata(KopiLua.LuaState state, object o)
        {
            Lua.LuaPushLightUserData(state, o);
        }

        internal static void luaL_newmetatable(KopiLua.LuaState state, string tname)
        {
            Lua.LuaLNewMetatable(state, tname);
        }

        internal static void lua_pushstring(KopiLua.LuaState state, string s)
        {
            Lua.LuaPushString(state, s);
        }

        internal static void lua_pushcfunction(KopiLua.LuaState state, KopiLua.LuaNativeFunction f)
        {
            Lua.LuaPushCFunction(state, f);
        }

        internal static void lua_settable(KopiLua.LuaState state, int idx)
        {
            Lua.LuaSetTable(state, idx);
        }

        internal static void lua_pushboolean(KopiLua.LuaState state, int b)
        {
            Lua.LuaPushBoolean(state, b);
        }

        internal static void lua_close(KopiLua.LuaState state)
        {
            Lua.LuaClose(state);
        }

        internal static void lua_pushnil(KopiLua.LuaState state)
        {
            Lua.LuaPushNil(state);
        }

        internal static int lua_next(KopiLua.LuaState state, int idx)
        {
            return Lua.LuaNext(state, idx);
        }

        internal static void lua_pushvalue(KopiLua.LuaState state, int idx)
        {
            Lua.LuaPushValue(state, idx);
        }

        internal static int abs_index(KopiLua.LuaState state, int i)
        {
            return Lua.AbsIndex(state, i);
        }

        internal static void lua_insert(KopiLua.LuaState state, int idx)
        {
            Lua.LuaInsert(state, idx);
        }

        internal static void lua_rawgeti(KopiLua.LuaState state, int idx, int n)
        {
            Lua.LuaRawGetI(state, idx, n);
        }

        internal static LuaType lua_type(KopiLua.LuaState state, int idx)
        {
            return (LuaType)Lua.LuaType(state, idx);
        }

        internal static void lua_rawseti(KopiLua.LuaState state, int idx, int n)
        {
            Lua.LuaRawSetI(state, idx, n);
        }

        internal static void lua_remove(KopiLua.LuaState state, int idx)
        {
            Lua.LuaRemove(state, idx);
        }

        internal static int lua_toboolean(KopiLua.LuaState state, int idx)
        {
            return Lua.LuaToBoolean(state, idx);
        }

        internal static double lua_tonumber(KopiLua.LuaState state, int idx)
        {
            return Lua.LuaToNumber(state, idx);
        }

        internal static string lua_tostring(KopiLua.LuaState state, int idx)
        {
            CharPtr cp = Lua.LuaToString(state, idx);

			if (cp == null)
			{
				return null;
			}

			// Returns the string as it is if it's a binary chunk.
			if (cp.ToString().StartsWith(Lua.LUA_SIGNATURE))
			{
				return cp.ToString(cp.chars.Length - 1);
			}

			// Decodes the string using UTF-8 if it's not a binary chunk.
			byte[] bytes = new byte[cp.chars.Length];
			for (int i = 0; i < cp.chars.Length; i++)
			{
				bytes[i] = (byte)cp.chars[i];
			}
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length - 1);
        }

        internal static int lua_gettop(KopiLua.LuaState state)
        {
            return Lua.LuaGetTop(state);
        }

        internal static int lua_getmetatable(KopiLua.LuaState state, int idx)
        {
            return Lua.LuaGetMetatable(state, idx);
        }

        internal static void luaL_getmetatable(KopiLua.LuaState state, string n)
        {
            Lua.LuaLGetMetatable(state, n);
        }

        internal static int lua_rawequal(KopiLua.LuaState state, int index1, int index2)
        {
            return Lua.LuaRawEqual(state, index1, index2);
        }

        internal static void lua_settop(KopiLua.LuaState state, int idx)
        {
            Lua.LuaSetTop(state, idx);
        }

        internal static int luaL_loadbuffer(KopiLua.LuaState state, string buff, int size, string name)
        {
            return Lua.LuaLLoadBuffer(state, buff, (uint)size, name);
        }

        internal static int luaL_loadbuffer(KopiLua.LuaState state, byte[] bytes, int size, string name)
        {
            return Lua.LuaLLoadBuffer(state, bytes, (uint)size, name);
        }

        internal static int lua_checkstack(KopiLua.LuaState state, int size)
        {
            return Lua.LuaCheckStack(state, size);
        }

        internal static int lua_pcall(KopiLua.LuaState state, int nargs, int nresults, int errfunc)
        {
            return Lua.LuaPCall(state, nargs, nresults, errfunc);
        }

        internal static void lua_setmetatable(KopiLua.LuaState state, int idx)
        {
            Lua.LuaSetMetatable(state, idx);
        }

        internal static int lua_upvalueindex(int i)
        {
            return Lua.LuaUpValueIndex(i);
        }

        internal static void lua_pushcclosure(KopiLua.LuaState state, KopiLua.LuaNativeFunction fn, int n)
        {
            Lua.LuaPushCClosure(state, fn, n);
        }

        internal static void lua_gettable(KopiLua.LuaState state, int idx)
        {
            Lua.LuaGetTable(state, idx);
        }

        internal static void lua_pushnumber(KopiLua.LuaState state, double n)
        {
            Lua.LuaPushNumber(state, n);
        }

        internal static int luaL_loadstring(KopiLua.LuaState state, string str)
		{
			return Lua.LuaLLoadString(state, str);
		}

        internal static void lua_pushlstring(KopiLua.LuaState state, byte[] str, int len)
		{
            // Makes a string out of the byte array.
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                sb.Append((char)str[i]);
            }
            string strval = sb.ToString();
            int lenval = sb.Length;

            // Pushes the string.
            Lua.LuaNetPushLString(state, strval, (uint)lenval);
		}

//#elif USE_WRC

//        public const int LUA_REGISTRYINDEX = -10000;
//        public const int LUA_ENVIRONINDEX = -10001;
//        public const int LUA_GLOBALSINDEX = -10002;

//        public const int LUA_MULTRET = -1;

//        public static int lua_upvalueindex(int i)
//        {
//            return LUA_GLOBALSINDEX - i;
//        }

//        public static int abs_index(lua51.LuaStateReference L, int i)
//        {
//            return (i > 0 || i <= LUA_REGISTRYINDEX) ? i : lua_gettop(L) + i + 1;
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_newstate")]
//        //public static extern IntPtr lua_newstate(LuaRuntime.LuaAllocator f, IntPtr ud);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_atpanic")]
//        //internal static extern void lua_atpanic(IntPtr luaState, IntPtr panicf);

//        public static int lua_checkstack(lua51.LuaStateReference L, int extra)
//        {
//            return lua51.Interop.LuaCheckStack(L, extra);
//        }

//        public static void lua_close(lua51.LuaStateReference L)
//        {
//            lua51.Interop.LuaClose(L);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_concat")]
//        //public static extern void lua_concat(IntPtr L, int n);

//        public static void lua_createtable(lua51.LuaStateReference L, int narr, int nrec)
//        {
//            lua51.Interop.LuaCreateTable(L, narr, nrec);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_equal")]
//        //public static extern int lua_equal(IntPtr L, int index1, int index2);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_error")]
//        //public static extern void lua_error(IntPtr luaState);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_gc")]
//        //public static extern int lua_gc(IntPtr L, LuaGcOperation what, int data);

//        public static void lua_getfield(lua51.LuaStateReference L, int index, string k)
//        {
//            lua51.Interop.LuaGetField(L, index, k);
//        }

//        public static void lua_getglobal(lua51.LuaStateReference L, string name)
//        {
//            lua_getfield(L, LUA_GLOBALSINDEX, name);
//        }

//        public static int lua_getmetatable(lua51.LuaStateReference L, int index)
//        {
//            return lua51.Interop.LuaGetMetatable(L, index);
//        }

//        public static void lua_gettable(lua51.LuaStateReference L, int index)
//        {
//            lua51.Interop.LuaGetTable(L, index);
//        }

//        public static int lua_gettop(lua51.LuaStateReference L)
//        {
//            return lua51.Interop.LuaGetTop(L);
//        }

//        public static void lua_insert(lua51.LuaStateReference L, int index)
//        {
//            lua51.Interop.LuaInsert(L, index);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_iscfunction")]
//        //public static extern int lua_iscfunction(IntPtr L, int index);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_lessthan")]
//        //public static extern int lua_lessthan(IntPtr L, int index1, int index2);

//        public static void lua_newtable(lua51.LuaStateReference L)
//        {
//            lua_createtable(L, 0, 0);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_newthread")]
//        //public static extern IntPtr lua_newthread(IntPtr L);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_newuserdata")]
//        //public static extern IntPtr lua_newuserdata(IntPtr L, UIntPtr size);

//        public static int lua_next(lua51.LuaStateReference L, int index)
//        {
//            return lua51.Interop.LuaNext(L, index);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_objlen")]
//        //public static extern UIntPtr lua_objlen(IntPtr L, int index);

//        public static int lua_pcall(lua51.LuaStateReference L, int nargs, int nresults, int errfunc)
//        {
//            return lua51.Interop.LuaNetPCall(L, nargs, nresults, errfunc);
//        }

//        public static void lua_pop(lua51.LuaStateReference L, int n)
//        {
//            lua_settop(L, -n - 1);
//        }

//        public static void lua_pushboolean(lua51.LuaStateReference L, int b)
//        {
//            lua51.Interop.LuaPushBoolean(L, b);
//        }

//        public static void lua_pushcclosure(lua51.LuaStateReference L, lua51.LuaCFunction fn, int n)
//        {
//            lua51.Interop.LuaPushCClosureDelegate(L, fn, n);
//        }

//        public static void lua_pushcfunction(lua51.LuaStateReference L, lua51.LuaCFunction f)
//        {
//            lua_pushcclosure(L, f, 0);
//        }

//        public static void lua_pushlightuserdata(lua51.LuaStateReference L, object p)
//        {
//            lua51.Interop.LuaPushLightUserData(L, p);
//        }

//        public static void lua_pushlstring(lua51.LuaStateReference L, byte[] s, uint len)
//        {
//            // Makes a string out of the byte array.
//            StringBuilder sb = new StringBuilder();
//            for (int i = 0; i < len; i++)
//            {
//                sb.Append((char)s[i]);
//            }
//            string strval = sb.ToString();
//            int lenval = sb.Length;

//            // Pushes the string.
//            lua51.Interop.LuaNetPushLString(L, sb.ToString(), len);
//        }

//        public static void lua_pushlstring(lua51.LuaStateReference L, byte[] s, int len)
//        {
//            lua_pushlstring(L, s, (uint)len);
//        }

//        public static void lua_pushlstring(lua51.LuaStateReference L, string s, int len)
//        {
//            if (s == null)
//            {
//                lua_pushnil(L);
//            }
//            else
//            {
//                var ulen = (uint)len;

//                var buffer = new byte[len];

//                for (int i = 0; i < s.Length; ++i)
//                {
//                    buffer[i] = unchecked((byte)s[i]);
//                }

//                lua_pushlstring(L, buffer, ulen);
//            }
//        }

//        public static void lua_pushstring(lua51.LuaStateReference L, string s)
//        {
//            if (s == null)
//            {
//                lua_pushnil(L);
//            }
//            else
//            {
//                lua_pushlstring(L, s, s.Length);
//            }
//        }

//        public static void lua_pushnil(lua51.LuaStateReference L)
//        {
//            lua51.Interop.LuaPushNil(L);
//        }

//        public static void lua_pushnumber(lua51.LuaStateReference L, double n)
//        {
//            lua51.Interop.LuaPushNumber(L, n);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushthread")]
//        //public static extern int lua_pushthread(IntPtr L);

//        public static void lua_pushvalue(lua51.LuaStateReference L, int index)
//        {
//            lua51.Interop.LuaPushValue(L, index);
//        }

//        public static int lua_rawequal(lua51.LuaStateReference L, int index1, int index2)
//        {
//            return lua51.Interop.LuaRawEqual(L, index1, index2);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawget")]
//        //public static extern void lua_rawget(IntPtr L, int index);

//        public static void lua_rawgeti(lua51.LuaStateReference L, int index, int n)
//        {
//            lua51.Interop.LuaRawGetI(L, index, n);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawset")]
//        //public static extern void lua_rawset(IntPtr L, int index);

//        public static void lua_rawseti(lua51.LuaStateReference L, int index, int n)
//        {
//            lua51.Interop.LuaRawSetI(L, index, n);
//        }

//        public static void lua_remove(lua51.LuaStateReference L, int index)
//        {
//            lua51.Interop.LuaRemove(L, index);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_replace")]
//        //public static extern void lua_replace(IntPtr L, int index);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_resume")]
//        //public static extern int lua_resume(IntPtr L, int narg);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_setallocf")]
//        //public static extern void lua_setallocf(IntPtr L, LuaRuntime.LuaAllocator f, IntPtr ud);

//        public static void lua_setfield(lua51.LuaStateReference L, int index, string k)
//        {
//            lua51.Interop.LuaSetField(L, index, k);
//        }

//        public static void lua_setglobal(lua51.LuaStateReference L, string name)
//        {
//            lua_setfield(L, LUA_GLOBALSINDEX, name);
//        }

//        public static void lua_setmetatable(lua51.LuaStateReference L, int index)
//        {
//            lua51.Interop.LuaSetMetatable(L, index);
//        }

//        public static void lua_settable(lua51.LuaStateReference L, int index)
//        {
//            lua51.Interop.LuaSetTable(L, index);
//        }

//        public static void lua_settop(lua51.LuaStateReference L, int index)
//        {
//            lua51.Interop.LuaSetTop(L, index);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_status")]
//        //public static extern int lua_status(IntPtr L);

//        public static int lua_toboolean(lua51.LuaStateReference L, int index)
//        {
//            return lua51.Interop.LuaToBoolean(L, index);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_tocfunction")]
//        //public static extern lua_CFunction lua_tocfunction(IntPtr L, int index);

//        public static double lua_tonumber(lua51.LuaStateReference L, int index)
//        {
//            return lua51.Interop.LuaNetToNumber(L, index);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_topointer")]
//        //public static extern IntPtr lua_topointer(IntPtr L, int index);

//        //public static IntPtr lua_tolstring(IntPtr L, int index, out uint len)
//        //{
//        //    return (IntPtr)lua51.Interop.LuaToLString(L, index, out len);
//        //}

//        public static byte[] lua_tostring(lua51.LuaStateReference L, int index)
//        {
//            uint len = 0;

//            return lua51.Interop.LuaToLStringArray(L, index, out len);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_tothread")]
//        //public static extern IntPtr lua_tothread(IntPtr L, int index);

//        public static object lua_touserdata(lua51.LuaStateReference L, int index)
//        {
//            return lua51.Interop.LuaToUserData(L, index);
//        }

//        public static LuaType lua_type(lua51.LuaStateReference L, int index)
//        {
//            return (LuaType)lua51.Interop.LuaType(L, index);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_typename")]
//        //[return: MarshalAs(UnmanagedType.LPStr)]
//        //public static extern string lua_typename(IntPtr L, LuaType tp);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_xmove")]
//        //public static extern void lua_xmove(IntPtr from, IntPtr to, int n);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_yield")]
//        //public static extern int lua_yield(IntPtr L, int nresults);

//        // Aux lib.

//        public static void luaL_getmetatable(lua51.LuaStateReference L, string name)
//        {
//            lua_getfield(L, LUA_REGISTRYINDEX, name);
//        }

//        public static int luaL_loadstring(lua51.LuaStateReference L, string s)
//        {
//            return lua51.Interop.LuaLLoadString(L, s);
//        }

//        public static int luaL_loadbuffer(lua51.LuaStateReference L, string s, int sz, string name)
//        {
//            return lua51.Interop.LuaNetLoadBuffer(L, s, (uint)sz, name);
//        }

//        public static int luaL_loadbuffer(lua51.LuaStateReference L, byte[] s, int sz, string name)
//        {
//            return lua51.Interop.LuaNetLoadBufferArray(L, s, (uint)sz, name);
//        }

//        public static int luaL_newmetatable(lua51.LuaStateReference L, string tname)
//        {
//            return lua51.Interop.LuaLNewMetatable(L, tname);
//        }

//        public static lua51.LuaStateReference luaL_newstate()
//        {
//            return lua51.Interop.LuaLNewState();
//        }

//        public static void luaL_openlibs(lua51.LuaStateReference L)
//        {
//            lua51.Interop.LuaLOpenLibs(L);
//        }

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_ref")]
//        //public static extern int luaL_ref(IntPtr L, int t);

//        //[DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_unref")]
//        //public static extern void luaL_unref(IntPtr L, int t, int r);

#else
        /*
         * Native Lua (DLL Import)
         */

    #if __IOS__
		const string LUA_DLL = "__Internal";
    #else
        const string LUA_DLL = "lua5.1.dll";
    #endif

        public const int LUA_REGISTRYINDEX = -10000;
        public const int LUA_ENVIRONINDEX = -10001;
        public const int LUA_GLOBALSINDEX = -10002;

        public const int LUA_MULTRET = -1;

        public static int lua_upvalueindex(int i)
        {
            return LUA_GLOBALSINDEX - i;
        }

        public delegate int lua_CFunction(IntPtr L);

        public static int abs_index(IntPtr L, int i)
        {
            return (i > 0 || i <= LUA_REGISTRYINDEX) ? i : lua_gettop(L) + i + 1;
        }

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_newstate")]
        public static extern IntPtr lua_newstate(LuaRuntime.LuaAllocator f, IntPtr ud);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_atpanic")]
        internal static extern void lua_atpanic(IntPtr luaState, IntPtr panicf);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_checkstack")]
        public static extern int lua_checkstack(IntPtr L, int extra);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_close")]
        public static extern void lua_close(IntPtr L);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_concat")]
        public static extern void lua_concat(IntPtr L, int n);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_createtable")]
        public static extern void lua_createtable(IntPtr L, int narr, int nrec);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_equal")]
        public static extern int lua_equal(IntPtr L, int index1, int index2);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_error")]
        public static extern void lua_error(IntPtr luaState);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_gc")]
        public static extern int lua_gc(IntPtr L, LuaGcOperation what, int data);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_getfield")]
        public static extern void lua_getfield(IntPtr L, int index, [MarshalAs(UnmanagedType.LPStr)] string k);

        public static void lua_getglobal(IntPtr L, string name)
        {
            lua_getfield(L, LUA_GLOBALSINDEX, name);
        }

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_getmetatable")]
        public static extern int lua_getmetatable(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_gettable")]
        public static extern void lua_gettable(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_gettop")]
        public static extern int lua_gettop(IntPtr L);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_insert")]
        public static extern int lua_insert(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_iscfunction")]
        public static extern int lua_iscfunction(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_lessthan")]
        public static extern int lua_lessthan(IntPtr L, int index1, int index2);

        public static void lua_newtable(IntPtr L)
        {
            lua_createtable(L, 0, 0);
        }

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_newthread")]
        public static extern IntPtr lua_newthread(IntPtr L);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_newuserdata")]
        public static extern IntPtr lua_newuserdata(IntPtr L, UIntPtr size);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_next")]
        public static extern int lua_next(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_objlen")]
        public static extern UIntPtr lua_objlen(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pcall")]
        public static extern int lua_pcall(IntPtr L, int nargs, int nresults, int errfunc);

        public static void lua_pop(IntPtr L, int n)
        {
            lua_settop(L, -n - 1);
        }

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushboolean")]
        public static extern void lua_pushboolean(IntPtr L, int b);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushcclosure")]
        public static extern void lua_pushcclosure(IntPtr L, lua_CFunction fn, int n);

        public static void lua_pushcfunction(IntPtr L, lua_CFunction f)
        {
            lua_pushcclosure(L, f, 0);
        }

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushlightuserdata")]
        public static extern void lua_pushlightuserdata(IntPtr L, IntPtr p);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushlstring")]
        public static extern void lua_pushlstring(IntPtr L, byte[] s, UIntPtr len);

        public static void lua_pushlstring(IntPtr L, string s, int len)
        {
            if (s == null)
            {
                lua_pushnil(L);
            }
            else
            {
                var ulen = new UIntPtr(checked((ulong)len));

                var buffer = new byte[len];

                for (int i = 0; i < s.Length; ++i)
                {
                    buffer[i] = unchecked((byte)s[i]);
                }

                lua_pushlstring(L, buffer, ulen);
            }
        }

        public static void lua_pushstring(IntPtr L, string s)
        {
            if (s == null)
            {
                lua_pushnil(L);
            }
            else
            {
                lua_pushlstring(L, s, s.Length);
            }
        }

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushnil")]
        public static extern void lua_pushnil(IntPtr L);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushnumber")]
        public static extern void lua_pushnumber(IntPtr L, double n);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushthread")]
        public static extern int lua_pushthread(IntPtr L);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_pushvalue")]
        public static extern void lua_pushvalue(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawequal")]
        public static extern int lua_rawequal(IntPtr L, int index1, int index2);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawget")]
        public static extern void lua_rawget(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawgeti")]
        public static extern void lua_rawgeti(IntPtr L, int index, int n);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawset")]
        public static extern void lua_rawset(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_rawseti")]
        public static extern void lua_rawseti(IntPtr L, int index, int n);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_remove")]
        public static extern void lua_remove(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_replace")]
        public static extern void lua_replace(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_resume")]
        public static extern int lua_resume(IntPtr L, int narg);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_setallocf")]
        public static extern void lua_setallocf(IntPtr L, LuaRuntime.LuaAllocator f, IntPtr ud);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_setfield")]
        public static extern void lua_setfield(IntPtr L, int index, [MarshalAs(UnmanagedType.LPStr)] string k);

        public static void lua_setglobal(IntPtr L, string name)
        {
            lua_setfield(L, LUA_GLOBALSINDEX, name);
        }

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_setmetatable")]
        public static extern int lua_setmetatable(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_settable")]
        public static extern void lua_settable(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_settop")]
        public static extern void lua_settop(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_status")]
        public static extern int lua_status(IntPtr L);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_toboolean")]
        public static extern int lua_toboolean(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_tocfunction")]
        public static extern lua_CFunction lua_tocfunction(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_tonumber")]
        public static extern double lua_tonumber(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_topointer")]
        public static extern IntPtr lua_topointer(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_tolstring")]
        public static extern IntPtr lua_tolstring(IntPtr L, int index, ref UIntPtr len);

        public static byte[] lua_tostring(IntPtr L, int index)
        {
            UIntPtr len = UIntPtr.Zero;

            var stringPtr = lua_tolstring(L, index, ref len);
            if (stringPtr == IntPtr.Zero)
            {
                return null;
            }

            var len32 = checked((int)len.ToUInt32());

            if (len32 == 0)
            {
                return new byte[0];
            }

            var buffer = new byte[len32];

            Marshal.Copy(stringPtr, buffer, 0, len32);

            return buffer;
        }

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_tothread")]
        public static extern IntPtr lua_tothread(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_touserdata")]
        public static extern IntPtr lua_touserdata(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_type")]
        public static extern LuaType lua_type(IntPtr L, int index);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_typename")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string lua_typename(IntPtr L, LuaType tp);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_xmove")]
        public static extern void lua_xmove(IntPtr from, IntPtr to, int n);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "lua_yield")]
        public static extern int lua_yield(IntPtr L, int nresults);

        // Aux lib.

        public static void luaL_getmetatable(IntPtr L, string name)
        {
            lua_getfield(L, LUA_REGISTRYINDEX, name);
        }

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_loadstring")]
        public static extern int luaL_loadstring(IntPtr L, [MarshalAs(UnmanagedType.LPStr)] string s);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_loadbuffer")]
        public static extern int luaL_loadbuffer(IntPtr L, [MarshalAs(UnmanagedType.LPStr)] string s, int sz, [MarshalAs(UnmanagedType.LPStr)] string name);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_loadbuffer")]
        public static extern int luaL_loadbuffer(IntPtr L, byte[] s, int sz, [MarshalAs(UnmanagedType.LPStr)] string name);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_newmetatable")]
        public static extern int luaL_newmetatable(IntPtr L, [MarshalAs(UnmanagedType.LPStr)] string tname);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_newstate")]
        public static extern IntPtr luaL_newstate();

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_openlibs")]
        public static extern void luaL_openlibs(IntPtr L);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_ref")]
        public static extern int luaL_ref(IntPtr L, int t);

        [DllImport(LUA_DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "luaL_unref")]
        public static extern void luaL_unref(IntPtr L, int t, int r);

#endif
    }
}
