/// Copyright (c) 2012 Ecma International.  All rights reserved. 
/**
 * @path ch15/15.2/15.2.3/15.2.3.6/15.2.3.6-4-207.js
 * @description Object.defineProperty - 'O' is an Array, 'name' is an array index named property, 'name' property doesn't exist in 'O' and [[Enumerable]] is absent in accessor descriptor 'desc', test [[Enumerable]] attribute of property 'name' is set to false (15.4.5.1 step 4.c)
 */


function testcase() {
        var arrObj = [];

        var setFunc = function (value) {
            arrObj.setVerifyHelpProp = value;
        };
        var getFunc = function () { };

        Object.defineProperty(arrObj, "0", {
            set: setFunc,
            get: getFunc,
            configurable: true
        });
        return accessorPropertyAttributesAreCorrect(arrObj, "0", getFunc, setFunc, "setVerifyHelpProp", false, true);
    }
runTestCase(testcase);