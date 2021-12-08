use chrono::{serde::ts_seconds, DateTime,Local, Utc};
use serde::{Deserialize, Serialize};

#[derive(Debug, Deserialize, Serialize)]
pub struct Task {
    pub text: String,
    #[serde(with = "ts_seconds")]
    pub created_at: DateTime<Utc>,
}

impl Task {
    pub fn new(text: String) -> Task {
        let create_at = Utc::now();
        Task {
            text,
            created_at,
        }
    }
}
