//
// Scripts.cs
//
// Author:
//       Chris Howie <me@chrishowie.com>
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
using System.IO;
using System.Reflection;
using System.Text;

namespace Eluant
{
    internal static class Scripts
    {
        private static string GetResource(string file)
        {
            using (var s = new StreamReader(typeof(Scripts).GetTypeInfo().Assembly.GetManifestResourceStream("Eluant." + file), Encoding.UTF8)) {
                return s.ReadToEnd();
            }
        }

        private static string bindingSupport;

        public static string BindingSupport
        {
            get {
                if (bindingSupport == null) {
                    // DW - 2016-02-19
                    // Removed because of problems with PCL
                    // bindingSupport = GetResource("BindingSupport.lua");
                    bindingSupport = @"
-- Make copies of global functions to prevent sandboxed scripts from altering
-- the behavior of Eluant bindings.
local real_pcall = pcall
local real_select = select
local real_error = error

-- Lua -> managed call support.
--
-- Since longjmp() can have bad effects on the CLR, we try to avoid generating
-- Lua errors in managed code.  Therefore we need a mechanism to raise errors
-- that does not rely on managed code.
--
-- The following code creates a function that is given a userdata representing
-- a CLR delegate and returns a Lua function that will call it.  The CLR code
-- is required to follow the same protocol as pcall(): the first return value is
-- true on success, followed by results, and false on failure, followed by the
-- error message.  The Lua proxy function will raise the error if necessary.
--
-- The other half of this protocol is implemented in C#, where the wrapper
-- responsible for unpacking Lua arguments into CLR types can capture exceptions
-- and propagate them as a false return flag.
local function process_managed_call_result(flag, flag2, ...)
    -- Because we pcall the wrapper, we have two levels of flags.  Outer is from
    -- pcall, inner is from the CLR side.
    
    -- pcall
    if not flag then real_error(flag2, ...) end
    
    -- CLR
    if not flag2 then real_error(...) end
    
    -- No error
    return ...
end

function eluant_create_managed_call_wrapper(fn)
    return function(...)
        return process_managed_call_result(real_pcall(fn, ...))
    end
end";
                }

                return bindingSupport;
            }
        }
    }
}

