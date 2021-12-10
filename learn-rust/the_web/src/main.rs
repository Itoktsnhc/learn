use actix_web::{get, post, web, App, HttpResponse, HttpServer, Responder};
use chrono::Local;
use mysql::{prelude::*};
use mysql::*;

#[derive(Debug)]
struct tt {
    id: i32,
    content: String,
}

#[get("/")]
async fn hello() -> impl Responder {
    HttpResponse::Ok().body("Hello world!")
}

#[get("/dt")]
async fn hello_dt() -> impl Responder {
    let now = Local::now().format("%Y-%m-%d %X").to_string();
    HttpResponse::Ok().body(now)
}

async fn manual_hello() -> impl Responder {
    HttpResponse::Ok().body("Hey there!")
}

#[get("/query")]
async fn query() -> impl Responder {
    let data = query_db();
    HttpResponse::Ok().body(format!("{:?}",data))
}
fn query_db() -> Vec<tt> {
    let conn_str = "mysql://root:123456@localhost:3306/for_test";
    let pool = Pool::new(Opts::from_url(conn_str).unwrap()).unwrap();
    let mut conn = pool.get_conn().unwrap();
    let res = conn
        .query_map("select * from tt", |(id, content)| tt { id, content })
        .unwrap();
    return res;
}

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    HttpServer::new(|| {
        App::new()
            .service(hello)
            .service(hello_dt)
            .service(query)
            .route("/hey", web::get().to(manual_hello))
    })
    .bind("127.0.0.1:8080")?
    .run()
    .await
}
