# CHEM_TRAI_CAY_OOP

Đây là project game Fruit Ninja được cải tiến theo hướng lập trình hướng đối tượng OOP bằng Unity và C#.

## Giới thiệu

Game cho phép người chơi dùng chuột để chém trái cây, tránh bom, tích điểm, tăng level và chơi lại khi Game Over.

## Chức năng chính

- Start Menu
- Play / Restart
- Game Over
- Score và Best Score
- Lives bằng trái tim
- Level tăng độ khó theo điểm
- Combo khi chém liên tiếp
- Trái cây thường
- GoldenFruit cộng nhiều điểm
- SlowFruit làm chậm thời gian
- HealFruit hồi mạng
- Bomb lớn làm mất mạng và trừ điểm
- SmallBomb chỉ trừ điểm
- Nhạc nền ở màn hình chờ
- Nhạc nền ở màn hình Game Over
- Âm thanh chém hụt và chém trúng

## OOP trong project

Project áp dụng các tính chất lập trình hướng đối tượng:

- Đóng gói
- Kế thừa
- Đa hình
- Trừu tượng
- Interface

## Sơ đồ kế thừa chính

```text
GameItem
├── Fruit
│   ├── NormalFruit
│   ├── GoldenFruit
│   ├── SlowFruit
│   └── HealFruit
└── Bomb
    └── SmallBomb
