import sqlite3
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.common.keys import Keys
import glob
from AppOpener import open

#database interface to send messages



class Message:
    def __init__(self, Content):
        self.Content = str(Content)
        self.Sender = "robot"
    def get_params(self):
        return (self.Content, self.Sender)


add_stmt = "INSERT INTO MessageData VALUES (NULL, ?, ?)"

#search google command
def google_search(message_list):
    if "google" in message_list[-1][1].lower()\
        and message_list[-1][2] == "human":
        #error checking
        query = message_list[-1][1].split("google")[1]
        if not len(query) > 1:
            error_msg = Message("must input a search").get_params()
            cnn.execute(add_stmt, error_msg)
            return False
        query = query[1:]

        #set the window to remain open
        options = Options()
        options.add_experimental_option("detach",True)

        #initialize driver window
        driver = webdriver.Chrome(service=Service(ChromeDriverManager().install()),options=options)
        driver.get("https://google.com")
        search_bar = driver.find_element("xpath","//textarea")
        search_bar.send_keys(query)
        search_bar.send_keys(Keys.RETURN)

        success_msg = Message("task performed").get_params()
        cnn.execute(add_stmt,success_msg)
        return True
    return False

#search filetype internally
def global_filesearch(message_list):
    if "internal search " in message_list[-1][1].lower()\
        and message_list[-1][2] == "human":
        if "." not in message_list[-1][1].lower().split("internal search")[1]:
            invalid_input_message = Message("invalid input: expects .<filetype>").get_params()
            cnn.execute(add_stmt, invalid_input_message)
            return True
        filename, filetype = message_list[-1][1].lower().split("internal search ")[1].split(".")
        files = glob.glob("*." + filetype)
        new_msgs = []
        for fl in files:
            new_msgs.append(Message(fl).get_params())
        if new_msgs:
            cnn.executemany(add_stmt, new_msgs)
            return True
        error_msg = Message("no files found").get_params()
        cnn.execute(add_stmt, error_msg)
        return True
    return False

def open_app(message_list):
    if "open" in message_list[-1][1]\
        and message_list[-1][2] == "human":
        query = message_list[-1][1].split("open")[1]
        if len(query) > 1:
            open(query)
            cnn.execute(add_stmt, Message("Attemting to open.").get_params())
            return True
    return False


def main(m):
    if global_filesearch(m):
        print("succesfully ran global")
    elif google_search(m):
        print("google search performed")
    elif open_app(m):
        print("app opened")
    else:
        cnn.execute(add_stmt, Message("invalid input").get_params())
        print("no function was triggered")

if __name__ == "__main__":
    with sqlite3.connect("C:\\Users\\chase ueltschey\\source\\repos\\CheronChatroom\\CheronChatroom\\messages.db") as cnn:
        main(list(cnn.execute('SELECT * FROM MessageData')))
        cnn.commit()