import ollama
import json

# 커스텀 모델 사용
response = ollama.chat(
    model='test-custom-llm:latest',  # 생성한 커스텀 모델
    messages=[
        {'role': 'user', 'content': '안녕하세요!'}
    ],
    format='json'  # JSON 형식 강제
)

# 결과 출력
print(response['message']['content'])

# JSON 파싱
try:
    result = json.loads(response['message']['content'])
    print("\n파싱된 결과:")
    print(f"메시지: {result['message']}")
    print(f"게임 상태: {result['game_state']}")
    print(f"가능한 액션: {result['actions']}")
except json.JSONDecodeError as e:
    print(f"JSON 파싱 실패: {e}")