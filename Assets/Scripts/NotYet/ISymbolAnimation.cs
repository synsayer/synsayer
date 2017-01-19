// Made by Paul at Dadasoft
// desc:: 라이온킹과 벨클래식의 심볼애니메이션  인터페이스 공통화를 통해 릴에서 접근할 수 있게 제작하였다

using UnityEngine;

public interface ISymbolAnimation
{
	bool Init(Transform parent);

	void Play();

	void Disable();

	void Idle();

	void RecycleGameObject();

	int nSymbolId { get; set; }
}