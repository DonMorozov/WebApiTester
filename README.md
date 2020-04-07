# WebApiTester
The simplest WebApi tester. Test script locates in .json file, request and response templates - in separate files.

# About tester
The tester is console .net Core open source F# application. It allows execute simple test script and test WebApi.
The tester reads and consequently executes script file.
Full path to script files expected in first command-line argument.
The script file contains named tests, that contains named steps.

Tests executes in alphabet order and steps into tests executes also alphabetically.
Please, **note it** when you will write your own test scripts.

Tester execute all tests in alphabet order.
For each test:
1. Tester resolves test-level parameters. It allow to use the same parameters  values in different test steps.
2. Tester executes all steps in alphabet order. For each steps:
2.1. Tester resolves all step-level parameters.
2.2. Step-level parameters is joined with resolved test-level parameters.
2.3. Tester read request-template file and substitute resolved params in template. Result will be sent to WebApi.
2.4. Tester read response-template file and substitute resolved params in template. Result is expected WebApi-response.
2.5. Request (result of 2.3) is sent to WebApi.
2.6. Response from WebApi is compared with expected response (result of 2.4).
2.7. If expected response is equal received response, then step is marked as "Success" and tester go to next step in current test.
2.8. If expected response is not equal received response, then step is marked as "Failure", test is marked as "Failure" and steps execution is interrupted.
2.9. If step can not be executed (e.g. Http request returns 404-error), then step is marked as "Can't execute", test is marked as "Failure" and steps execution is interrupted.
3. If all steps of test are marked as "Success", then test is marked as "Success".
4. If any step of test is marked as "Failure" or "Can't execute", then test is marked as "Failure".

# Test script file format
Test script file is json-file.
## The fields of script file
**TemplatePath** Path to request- and response- files
**URL** URL of tested WebApi
**Tests** Dictionary of tests configurations to executes **Important! tests will start in alphabet order**
**Tests.[].URL** URL of tested WebApi, if it presents, then script-level parameter URL is ignored.
**Tests.[].Params** Dictionary of test-level parameters.
**Tests.[].Steps** Dictionary of steps of test for execute (will be executed in alphabetically order).
**Tests.[].Steps.[].URL** URL of tested WebApi, if it presents, then script-level and test-level parameter URL is ignored.
**Tests.[].Steps.[].Request** Request template file name.
**Tests.[].Steps.[].Response** Response template file name.
**Tests.[].Steps.[].Params** Test-level parameters.

## Parameters
Parameters are used for parametrizing of request- and response- templates.
If template contains parameter name into curves, then this name with this curves will be replaced by parameter value.
There are some types of parameters:
- value-parameter (any parameter value in script, except reserved) - name in template will be directly replaced by parameter value;
- guid-parameter (parameter value in script is **~Guid** ) - name in template will be replaced by guid, that will be generated during parameter resolution;
- date-parameter (parameter value in script is **~Date** or **~DateTime** ) - name in template will be replaced by date (or date and time) of parameter resolution in ISO-format.

# Sample tester work
## Solution structure
Solution contains two projects.
1. **WebApiTester** - The tester.
2. **WebApiSample** - Sample of WebApi for test, debug and demo of the Tester.

## Example of test script and templates
Example script and templates locate in "Test" folder.
WebApiSample.test.json - example of test script
Templates examples are:
- HelloRequest.xml
- HelloResponse.xml
- DialogRequest.xml
- DialogResponse.xml

## How to see tester work
1. Run WebApiSample.
2. Run WebApiTester (command-line argument should be full-path to WebApiSample.test.json).
Note, that WebApiSample-service should be in URL-field of WebApiSample.test.json.