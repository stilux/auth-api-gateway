from locust import HttpUser, task, between
from faker import Faker
import time

fake = Faker()


class CommonUser(HttpUser):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.products = None

    @task(1)
    def search(self):
        url = '/product/find?q=' + fake.word() + '&take=5'
        print(url)
        self.products = self.client.get(url, name='product_search').json()

        time.sleep(2)

        if self.products is not None:
            for product in self.products:
                self.get_product_id(product['id'])
                time.sleep(2)

    def get_product_id(self, product_id):
        self.client.get('/product/' + str(product_id), name='product_id')

    wait_time = between(5, 10)
