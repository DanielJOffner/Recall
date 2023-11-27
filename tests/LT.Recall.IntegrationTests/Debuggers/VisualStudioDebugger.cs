using EnvDTE;
using Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;

namespace LT.Recall.IntegrationTests.Debuggers
{
    // https://stackoverflow.com/questions/14205933/how-do-i-get-the-dte-for-running-visual-studio-instance
    // https://stackoverflow.com/questions/11811856/attach-debugger-in-c-sharp-to-another-process
    public class VisualStudioDebugger : IDebugger
    {
        public void Attach(int pid)
        {
            var instances = GetInstances();
            var dte = instances.First();
            MessageFilter.Register();

            IEnumerable<Process> processes = dte.Debugger.LocalProcesses.OfType<Process>();
            var process = processes.SingleOrDefault(x => x.ProcessID == pid);
            if (process != null)
            {
                process.Attach();
            }
            else
            {
                throw new Exception($"Process not found {pid}");
            }
        }

        private IEnumerable<DTE> GetInstances()
        {
            IRunningObjectTable rot;
            IEnumMoniker enumMoniker;
            int retVal = GetRunningObjectTable(0, out rot);

            if (retVal == 0)
            {
                rot.EnumRunning(out enumMoniker);

                uint fetched = uint.MinValue;
                IMoniker[] moniker = new IMoniker[1];
                while (enumMoniker.Next(1, moniker, out fetched) == 0)
                {
                    IBindCtx bindCtx;
                    CreateBindCtx(0, out bindCtx);
                    string displayName;
                    moniker[0].GetDisplayName(bindCtx, null, out displayName);
                    Console.WriteLine("Display Name: {0}", displayName);
                    bool isVisualStudio = displayName.StartsWith("!VisualStudio");
                    if (isVisualStudio)
                    {
                        object obj;
                        rot.GetObject(moniker[0], out obj);
                        var dte = obj as DTE;
                        yield return dte!;
                    }
                }
            }
        }

        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);


        private class MessageFilter : IOleMessageFilter
        {
            //
            // Class containing the IOleMessageFilter
            // thread error-handling functions.

            // Start the filter.

            //
            // IOleMessageFilter functions.
            // Handle incoming thread requests.

            #region IOleMessageFilter Members

            int IOleMessageFilter.HandleInComingCall(int dwCallType,
                                                     IntPtr hTaskCaller, int dwTickCount, IntPtr
                                                                                              lpInterfaceInfo)
            {
                //Return the flag SERVERCALL_ISHANDLED.
                return 0;
            }

            // Thread call was rejected, so try again.
            int IOleMessageFilter.RetryRejectedCall(IntPtr
                                                        hTaskCallee, int dwTickCount, int dwRejectType)
            {
                if (dwRejectType == 2)
                // flag = SERVERCALL_RETRYLATER.
                {
                    // Retry the thread call immediately if return >=0 & 
                    // <100.
                    return 99;
                }
                // Too busy; cancel call.
                return -1;
            }

            int IOleMessageFilter.MessagePending(IntPtr hTaskCallee,
                                                 int dwTickCount, int dwPendingType)
            {
                //Return the flag PENDINGMSG_WAITDEFPROCESS.
                return 2;
            }

            #endregion

            public static void Register()
            {
                IOleMessageFilter newFilter = new MessageFilter();
                IOleMessageFilter oldFilter = null!;
                CoRegisterMessageFilter(newFilter, out oldFilter);
            }

            // Done with the filter, close it.
            public static void Revoke()
            {
                IOleMessageFilter oldFilter = null!;
                CoRegisterMessageFilter(null!, out oldFilter);
            }

            // Implement the IOleMessageFilter interface.
            [DllImport("Ole32.dll")]
            private static extern int
                CoRegisterMessageFilter(IOleMessageFilter newFilter, out
                                                                         IOleMessageFilter oldFilter);
        }

        [ComImport, Guid("00000016-0000-0000-C000-000000000046"),
         InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleMessageFilter
        {
            [PreserveSig]
            int HandleInComingCall(
                int dwCallType,
                IntPtr hTaskCaller,
                int dwTickCount,
                IntPtr lpInterfaceInfo);

            [PreserveSig]
            int RetryRejectedCall(
                IntPtr hTaskCallee,
                int dwTickCount,
                int dwRejectType);

            [PreserveSig]
            int MessagePending(
                IntPtr hTaskCallee,
                int dwTickCount,
                int dwPendingType);
        }
    }
}