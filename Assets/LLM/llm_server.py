import json
import ollama
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import List
import os

"""
rl: /Users/pcy/Documents/ReinforcementLearning/rl/bin/python

프롬프트 변경하는 방법: ollama create {name} -f ./Assets/LLM/ManagementAgent_prompt
"""

my_model = ['lab:latest', 'smith:latest', 'house:latest']
item_storage_path = './Assets/LLM/items_storage.json'
'''
message = """합성할 아이템 수: 2
아이템 정보 1: {물, 30, 투명한 물방울, 재료}
아이템 정보 2: {흙, 20, 검은색의 진흙, 재료ㅌ}"""

response = ollama.chat(
    model=my_model,  # 생성한 커스텀 모델
    messages=[
        {'role': 'user', 'content': message}
    ],
    format='json'  # JSON 형식 강제
)
print(response['message']['content'])
'''
'''
# 스트리밍
for chunk in ollama.chat(
    model=my_model,
    messages=[{'role': 'user', 'content': '긴 이야기를 해주세요'}],
    stream=True
):
    print(chunk['message']['content'], end='', flush=True)
'''

app = FastAPI()
class RequestBody(BaseModel):
    type: int
    message: str
class ResponseBody(BaseModel):
    name: str
    price: float
    description: str
    category: str
    reasoning: str
@app.post("/create_item", response_model=ResponseBody)
async def create_item(request_body: RequestBody):
    try:
        message = request_body.message
        type = request_body.type
        print(message)
        response = ollama.chat(
            model=my_model[type],
            messages=[
                {'role': 'user', 'content': message}
            ],
            format='json'
        )

        result = response['message']['content']
        parsed_result = json.loads(result)
        print(parsed_result)
        if not os.path.exists(item_storage_path):
            os.makedirs(item_storage_path, exist_ok=True)
        
        f = open(item_storage_path, 'a', encoding='utf-8')
        f.write(json.dumps(parsed_result, ensure_ascii=False) + '\n')
        f.close()
        
        return ResponseBody(
            name=parsed_result['name'],
            price=parsed_result['price'],
            description=parsed_result['description'],
            category=parsed_result['category'],
            reasoning=parsed_result['reasoning']
        )
    except json.JSONDecodeError as e:
        raise HTTPException(status_code=500, detail=f"JSON 파싱 실패: {e}")
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
    
if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
