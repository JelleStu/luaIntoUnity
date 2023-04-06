lu = require('luaunit')
testlua = {}
 function testlua:add(v1,v2)
    -- add positive numbers
    -- return 0 if any of the numbers are 0
    -- error if any of the two numbers are negative
    if v1 < 0 or v2 < 0 then
        error('Can only add positive or null numbers, received '..v1..' and '..v2)
    end
    if v1 == 0 or v2 == 0 then
        return 0
    end
    return v1+v2
end

function testlua:testAddPositive()
    lu.assertEquals(testlua:add(1,1),3)
end

function testlua:testAddPositive2()
    lu.assertEquals(testlua:add(1,1),2)
end

TestAdd = {}
    function TestAdd:testAddPositive()
        lu.assertEquals(add(1,1),2)
    end

    function TestAdd:testAddZero()
        lu.assertEquals(add(1,0),0)
        lu.assertEquals(add(0,5),0)
        lu.assertEquals(add(0,0),0)
    end

    function TestAdd:testAddError()
        lu.assertErrorMsgContains('Can only add positive or null numbers, received 2 and -3', add, 2, -3)
    end

    function TestAdd:testAdder()
        f = adder(3)
        lu.assertIsFunction( f )
        lu.assertEquals( f(2), 5 )
    end
-- end of table TestAdd
-- os = {}
-- function os:exit(f)

-- end
os.exit()

return testlua