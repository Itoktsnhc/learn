use std::net::TcpListener;

#[tokio::test]
async fn health_check_works() {
    let addr = spawn_app();
    let path = format!("{}/health_check", addr);
    let client = reqwest::Client::new();
    let resp = client
        .get(path)
        .send()
        .await
        .expect("Failed to execute request.");
    assert!(resp.status().is_success());
    assert_eq!(Some(0), resp.content_length());
}

fn spawn_app() -> String {
    let listener = TcpListener::bind("127.0.0.1:0").expect("Failed to bind random port");
    let port = listener.local_addr().unwrap().port();
    let server = zero2prod::run(listener).expect("Failed to bind address");
    let _ = tokio::spawn(server);
    format!("http://127.0.0.1:{}", port)
}
