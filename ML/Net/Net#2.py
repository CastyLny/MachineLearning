import tensorflow as tf
import matplotlib.pyplot as plt
import numpy as np


data = tf.keras.datasets.mnist
(train_images, train_labels), (test_images, test_labels)= data.load_data()

labels =  np.unique(train_labels)
print(labels)
train_images = train_images/255.0
test_images = test_images/255.0

model = tf.keras.Sequential([
    tf.keras.layers.Flatten(input_shape=(28,28)),
    tf.keras.layers.Dense(128, activation='relu'),
    tf.keras.layers.Dense(10)
])

model.compile(optimizer='adam', loss=tf.keras.losses.SparseCategoricalCrossentropy(from_logits=True),
              metrics=['accuracy']
              )


model.fit(train_images, train_labels, epochs=20)

test_loss, test_acc = model.evaluate(test_images, test_labels, verbose=2)
print(test_acc)
result = model.predict(test_images)
result = model.predict(test_images)

number = 4

index = 0
previousValue = 0
print(list(result)[number])

plt.figure()
plt.imshow(test_images[number])
plt.colorbar()
plt.grid(False)
print(labels[np.argmax(list(result)[number])])
plt.show()