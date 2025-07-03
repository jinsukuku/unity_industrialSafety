# 🧠 Git LFS (Git Large File Storage) 간단 사용법

GitHub에서는 **100MB**가 넘는 파일은 푸시할 수 없습니다.
또한, **50MB**가 넘는 파일은 경고가 발생하무로 **대용량 파일은 Git LFS로 관리하는 것이 좋습니다.**

---

## ✅ 1. Git LFS 설치

* 공식 홈페이지: [https://git-lfs.github.com](https://git-lfs.github.com)
    * Homebrew: `brew install git-lfs`
* 설치 완료 후, 터미널에서 아래 명령어 입력:

```bash
git lfs install
```

---

## ✅ 2. 추적할 파일 유형 등록

매우 대형의 `.mp4` 파일을 Git LFS로 관리하려면

```bash
git lfs track "*.mp4"
```

* 이 명령어는 `.gitattributes` 파일을 생성하거나 수정합니다.

---

## ✅ 3. Git에 파일 추가 및 커미트

```bash
git add .gitattributes
git add firstscene.mp4
git commit -m "Track mp4 files using Git LFS"
git push origin main
```

---

## ✅ 4. 주의 사항

* 협업자도 규칙적으로 **Git LFS가 설치되어 있어야 합니다**
* LFS로 관리되는 파일은 일반 Git 히스토리에 들어가지 않습니다

---

## ✅ 자주 사용하는 명령어 정보

| 명령어                     | 설명                 |
| ----------------------- | ------------------ |
| `git lfs install`       | LFS 기능을 현재 시스템에 적용 |
| `git lfs track "*.확장자"` | 특정 확장자 파일을 LFS로 추적 |
| `git lfs ls-files`      | 현재 추적 중인 LFS 파일 확인 |
| `git lfs uninstall`     | LFS 기능 해제          |

---

## 📌 참고

* GitHub: [https://docs.github.com/en/repositories/working-with-files/managing-large-files/about-large-files-on-github](https://docs.github.com/en/repositories/working-with-files/managing-large-files/about-large-files-on-github)
* Git LFS Docs: [https://git-lfs.github.com](https://git-lfs.github.com)
