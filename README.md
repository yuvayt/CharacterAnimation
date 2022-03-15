# Character_Animation
 
## Day 1
- Hoàn thành tạo nhân vật từ 6 hình khối
- Hoàn thành 10 Animation cho nhân vật bao gồm cả các Animation con như Move_forward, Move_backward, Move_left, Move_right, Falling_forward, Falling_backward.
- Hoàn thành tạo Animator với trạng thái mặc định là Idle cùng với các đường nối, để các hoạt động có thể nối tiếp nhau. Có sử dụng tham số speed dạng float để có thể tùy biến tốc độ thực hiện của animation chạy.


## Day 2
- Tạo ra map size 500x500 (Do em thấy map size 100 x 100 không đủ không gian để sinh ra 500 vật cản ngẫu nhiên).
- Hoàn thành sinh ngẫu nhiên ra các vật cản với kích thước ngẫu nhiên bằng cách random một điểm bất kì trong tọa độ cho trước rồi sử dụng checkSphere để kiểm tra xem trong bán kính 10 đơn vị đã tồn tại vật cản hoặc người chơi chưa. Nếu rồi thì sẽ tiếp tục random điểm khác để tìm vị trí sinh vật cản ngẫu nhiên. Sau 20 lần random mà chưa tìm được vị trí sinh vật cản thì sẽ dừng. 
- Xử lí controller:
     - Có thể sử dụng các mũi tên hoặc wasd để điều khiển di chuyển nhân vật. Ấn Space để nhân vật nhảy.
     - Nhân vật sẽ có tốc độ tối thiểu và tốc độ tối đa. Nhân vật luôn bắt đầu di chuyển ở tốc độ tối thiếu và sẽ tăng dần đến tốc độ tối đa nếu nhân vật tiếp tục di chuyển. Nếu nhân vật không di chuyển thì tốc dộ sẽ giảm và animation sẽ chuyển dần từ chạy sang đi bộ và khi nhật vật không còn di chuyển nữa thì chuyển về trạng thái idle. Khi vượt qua tốc độ trung bình thì nhân vật sẽ chuyển animation từ đi bộ thành chạy, chạy càng lâu speed càng tăng và tốc dộ thực hiện animaion run cũng tăng ( tay vung nhanh hơn, chân chạy nhanh hơn).
     - Đã hoàn thành việc xác định hướng ngã và thực hiện animation khi gặp phải vật to hoặc nhỏ hơn so với nhân vật.

## Issues
- [x] #1
- [x] #2
- [ ] #8
- [ ] #6
- [ ] #7
