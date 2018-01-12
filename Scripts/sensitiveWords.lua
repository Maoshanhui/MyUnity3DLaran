sensitiveWords = {}
require "game/tools/sensitiveWordsTable"  
  
local chat_dict  
local chat_leaves = {}
local record
--构造字典树  
function sensitiveWords.init_chat_dict()  
    for i = 1, #record do
        if record[i] ~= nil and record[i].text ~= nil then 
            local word = record[i].text  
            local t = chat_dict  
            for j = 1, #word do   
                local c = string.byte(word, j)  
                if not t[c] then  
                    t[c] = {}  
                end  
                t = t[c]  
            end           
            chat_leaves[word] = true  
        end
    end  
end  

function sensitiveWords.addSensitiveWord( wordTable )
    if not chat_dict then  
        chat_dict = {}  
        record = _sensitiveTable 
    end  

    local num = #record;

    if wordTable == nil then 
        return 
    end

    local addNum = #wordTable;

    for i = 1, addNum  do
        local hasValue = false;
        for j = 1, num do 
            if record[j] ~= nil and record[j].text ~= nil and record[j].text == wordTable[i] then
                hasValue = true;
                break;
            end
        end
        if hasValue == false then
            num = num + 1;
            record[num] = {};
            record[num].text = wordTable[i];
        end
    end
end

function sensitiveWords.deleteSensitiveWord( wordTable )
    local num = #record;

    if wordTable == nil or #wordTable == 0 then 
        return 
    end

    local deleteNum = #wordTable;

    for i = 1, num do
        for j = 1, deleteNum do
            if record[i] ~= nil and record[i].text ~= nil and record[i].text == wordTable[j] then
                record[i] = nil;
            end
        end
    end
end
  
-- 匹配判断是否为敏感词  
function sensitiveWords.isSensitiveWord(msg)   
    -- if not chat_dict then  
    --     sensitiveWords.init_chat_dict()  
    -- end  
    local matchs = {}  
    for i = 1, #msg do  
        local p = i  
        local q = p  
        local t = chat_dict  
        while true do  
            if not t[string.byte(msg,q)] then  
                q = q - 1  
                break  
            end  
            t = t[string.byte(msg, q)]  
            q = q + 1  
        end  
        if q >= p then  
            local str = string.sub(msg, p, q)  
            if chat_leaves[str] then  
                table.insert(matchs, {b = p, e = q, l = (q - p + 1)})  
            end  
        end  
    end  
    
    if #matchs == 0 then
        return true;
    else
        return false;
    end
end  

-- 敏感词替换为星号，用于聊天的敏感词过滤
function sensitiveWords.ChatFilter(msg)   
    -- if not chat_dict then  
    --     sensitiveWords.init_chat_dict()  
    -- end  
    local matchs = {}  
    for i = 1, #msg do  
        local p = i  
        local q = p  
        local t = chat_dict  
        while true do  
            if not t[string.byte(msg,q)] then  
                q = q - 1  
                break  
            end  
            t = t[string.byte(msg, q)]  
            q = q + 1  
        end  
        if q >= p then  
            local str = string.sub(msg, p, q)  
            if chat_leaves[str] then  
                table.insert(matchs, {b = p, e = q, l = (q - p + 1)})  
            end  
        end  
    end  

    local str = msg  
    for _,v in ipairs(matchs) do  
        str = string.sub(str, 1, v.b - 1) .. string.rep("*", v.l) .. string.sub(str, v.e + 1)  
    end

    return str;
end  