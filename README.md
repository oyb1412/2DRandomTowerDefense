### 

# 2DRandom Tower Defense


---

## Description

---


- 🔊프로젝트 소개

  2DRandom Tower Defense는 랜덤하게 생성되는 타워를 강화해 적을 막아내는 2D 타워디펜스 게임입니다. 타워의 설치, 제거, 이동 등 타일맵과의 상호작용을 구현하는대 중점을 두었습니다.

       

- 개발 기간 : 2024.01.05 - 2024.01.15

- 🛠️사용 기술

   -언어 : C#

   -엔진 : Unity Engine

   -데이터베이스 : 로컬

   -개발 환경: Windows 10, Unity 2021.3.10f1



- 💻구동 화면

![스크린샷(4)](https://github.com/oyb1412/2DRandomTowerDefense/assets/154235801/0c751e94-53f7-4025-8fbb-8875192c01fb)

## 목차

---

- 기획 의도
- 핵심 로직


### 기획 의도

---

- 타일맵과의 상호작용이 가능한 타워디펜스 게임 제작



### 핵심 로직

---
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)
### ・타일맵과의 상호작용

타일맵과의 상호작용을 구현해 타일맵 선택, 타워 설치등의 로직을 구현.

![1](https://github.com/oyb1412/2DRandomTowerDefense/assets/154235801/4df72aec-f3a0-4655-b3a4-cffb518993fd)
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)


### ・Navmesh를 이용하지 않은 적 유닛의 이동 로직

최적화를 위해 NavmeshAgent를 이용하지 않고 이동 로직 구현

![2](https://github.com/oyb1412/2DRandomTowerDefense/assets/154235801/a639d64f-11be-4fed-83fd-02e07a452273)
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)

### ・타워 랜덤생성, 판매, 합성, 위치변경

랜덤 타워디펜스의 핵심적인 로직인 타워 랜덤생성과 합성, 타워의 위치변경의 구현
![11](https://github.com/oyb1412/2DRandomTowerDefense/assets/154235801/5c73f54c-7bbe-4d65-8fc3-efcc08f56c47)
![12](https://github.com/oyb1412/2DRandomTowerDefense/assets/154235801/6e9585f5-b678-4721-b95e-2f09d4287c61)
![15](https://github.com/oyb1412/2DRandomTowerDefense/assets/154235801/bae07b17-273d-47f8-a2ca-bc1eec3f4aa7)
![그림8](https://github.com/oyb1412/2DRandomTowerDefense/assets/154235801/3ac49119-61ea-4615-9c8d-6a1050f6df46)
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)
