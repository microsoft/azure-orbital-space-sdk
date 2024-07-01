# Debug Client
Debug Client is client used to debug service functionality or integration with other services

# Debug Client vs HostSvc

HostSvc is a simple representation of a host service for receiving and processing requests. DebugClient is a sample representation of a payload app to trigger requests for the host. A dedicated debug pod is used for both projects to better simulate a production environment.

Both processes are DebugClient and HostSvc is ready to start as the VSCode task 'HostSvc and DebugClient'

## Run Tests:
1. Open target respository with a debugger in its Dev Container
1. Open the "Run and Debug" menu (CTRL + SHIFT + D)

1. From the launch configuration dropdown, select "Integration Tests - Run"

    ![Launch Configuration Options](../../img/f5-experiences.png)

1. Select the Start Icon ▶ (or press F5) to begin debugging the service and running the tests
    ![Launch Configuration Options](../../img/click_f5.png)
1. Your terminal will show the tasks executed as described in [tasks.json]

1. The Debug Console (CTRL + SHIFT + Y) will become active, select the "Integration Tests - Client Run" session from the dropdown

    ![Debug Console Output](../../img/debug_client_integration_run.png)
2. A successful test run only should emit something like this to the debug console
    ```plaintext
        Starting test execution, please wait...
        A total of 1 test files matched the specified pattern.
        /workspaces/hostsvc-logging/test/integrationTests/bin/Debug/net6.0/integrationTests.dll
        [xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.4.5+1caef2f33e (64-bit .NET 6.0.16)
        [xUnit.net 00:00:00.60]   Discovering: integrationTests
        [xUnit.net 00:00:00.63]   Discovered:  integrationTests
        [xUnit.net 00:00:00.64]   Starting:    integrationTests
        Waiting for 'hostsvc-Logging-test-host' to come online...
        hostsvc-Logging-test-host is online.  Starting tests
        Removing all files from Output directory
        [xUnit.net 00:00:04.19]   Finished:    integrationTests
        Passed integrationTests.LogMessageTests.SendDirectLogMessage [1 s]
        Passed integrationTests.LogMessageTests.SendDirectLogMessageBelowThreshold [100 ms]

        Test Run Successful.
        Total tests: 2
            Passed: 2
        Total time: 4.6688 Seconds
    ```


## Debugging with breakpoints :
1. Open this target respository with a debugger in its Dev Container 
2. Set a breakpoint on functions you want to debug
3. Click in the gutter area to the left of a line number, a circle 🔴 will appear for lines with breakpoints set![breakpoint.png](../../img/breakpoint.png) 
4. Open the "Run and Debug" menu (CTRL + SHIFT + D)
5. From the launch configuration dropdown, select debugService of choice such as "Integration Tests - Run"
    ![Launch Configuration Options](../../img/f5-experiences.png)
6. Select the Start Icon ▶ (or press F5) to begin debugging the service and running the tests
    ![Launch Configuration Options](../../img/click_f5.png)

7. The breakpoint will be hit and execution of the application will pause
    ![Breakpoint hit](../../img/integration-test-breakpoint-hit.png)
8. You can inspect the state of the application, view the call stack, and interact with the application (skip if running tests only)

9.  Remove your breakpoint, or move it around, explore how the application works

10. Select the Continue Icon ⏯️ (or press F5) to continue execution

    ![Continue Icon](../../img/integration-test-continue.png)

11. Remember, once finished, stop the debugger by selecting the Stop Icon 🟥 (Shift + F5)
    ![Continue Icon](../../img/stop_debug.png)

    **Note**: This is a **required step**, failure to stop the services will result in unknown pod states


## Notes
1. VSCode tests fo not work
   VSCode's "Run All Tests/Debug All Tests" and "Run Test / Debug Test" does not work as the background process runs within the context of the devcontainer. This is incompatible with kubernetes pod. This inoperability is expected behavior.