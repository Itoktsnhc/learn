mod cli;
use structopt::StructOpt;

fn main() {
    let args = cli::CommandLineArgs::from_args();
    println!("{:?}", args);
}
